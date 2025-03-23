# This script is used to build the project for production
# The script will stop if any error occurs


# Variables
TARGET_USER="ksk"
SSH_KEY="$HOME/.ssh/id_rsa_rpi"
TARGET_HOST="192.168.1.106"
PRODUCT_DIR="/opt/clock/bin"
STAGING_DIR="/opt/clock/stage"

# Function to print error message and exit
print_error_and_exit() {
    tput setaf 1
    echo "Error: $1"
    tput setaf 2
    echo "Exiting..."
    echo ""
    exit 1
}

PUBLISH_DIR=$1 # Build Publish directory

# Check if PUBLISH_DIR is set
if [ -z "$PUBLISH_DIR" ]; then
    print_error_and_exit "PUBLISH_DIR is not set"
fi

# Ensure build is ready
if [ ! -d "$PUBLISH_DIR" ]; then
    print_error_and_exit "Error: Build directories not found. Ensure 'dotnet publish' has been run."
fi

#check if the product directory exists and create it if it doesn't
printf "Checking the product directory on the target device..."
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "[ -d $PRODUCT_DIR ]"
if [ $? -ne 0 ]; then
    ssh $TARGET_USER@$TARGET_HOST "mkdir -p $PRODUCT_DIR"
fi
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "[ -d $PRODUCT_DIR ]"
if [ $? -ne 0 ]; then
    print_error_and_exit "Cannot create the product directory "$PRODUCT_DIR"."
else
    echo " OK."
fi

#check if the staging directory exists and create it if it doesn't
printf "Checking the staging directory on the target device..."
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "[ -d $STAGING_DIR ]"
if [ $? -ne 0 ]; then
    ssh $TARGET_USER@$TARGET_HOST "mkdir -p $STAGING_DIR"
fi
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "[ -d $STAGING_DIR ]"
if [ $? -ne 0 ]; then
    print_error_and_exit "Cannot create the staging directory "$STAGING_DIR"."
else
    echo " OK."
fi
echo ""

# Transfer files
printf "Transferring files to the server..."

# Create the target directory
ssh -i "$SSH_KEY" "$TARGET_USER@$TARGET_HOST" "mkdir -p $STAGING_DIR"

# Use rsync to transfer files and show real-time progress
# `stdbuf` is used to adjust buffering behavior to display output line-by-line
stdbuf -oL rsync -avz --progress --bwlimit=10 -e "ssh -i $SSH_KEY" "$PUBLISH_DIR/" "$TARGET_USER@$TARGET_HOST:$STAGING_DIR/" | \
while IFS= read -r line; do
    # This condition checks for lines with transfer progress information
    if [[ "$line" =~ (file|to-check|consider|up to date) ]]; then
        # Clear the line and print the current progress
        printf "\rTransferring files to the server... %s" "$line"
    fi
done

# Clear the line before printing the final success message
printf "\r%80s\r" ""  # Clear the line

# Final message when the transfer is done
echo "Transferring files to the server... OK."
echo ""

# Stop existing service
printf "Stopping service..."
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "sudo systemctl stop clock.service"
if [ $? -ne 0 ]; then
    print_error_and_exit "Cannot stop the service."
else
    echo " OK."
fi

# Replace old binaries with new ones
printf "Replacing binaries..."
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "rm -rf $PRODUCT_DIR"
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "mkdir -p $PRODUCT_DIR"
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "cp -r $STAGING_DIR/* $PRODUCT_DIR/"
echo " OK."

# Start the service
printf "Starting service..."
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "sudo systemctl start clock.service"
if [ $? -ne 0 ]; then
    print_error_and_exit "Cannot start the service."
else
    echo " OK."
fi
echo ""
