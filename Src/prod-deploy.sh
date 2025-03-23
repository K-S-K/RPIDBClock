# This script is used to build the project for production
# The script will stop if any error occurs


# Variables
TARGET_USER="ksk"
SSH_KEY="$HOME/.ssh/id_rsa_rpi"
TARGET_HOST="192.168.1.106"
TARGET_DIR="/opt/clock/bin"
STAGE_DIR="/opt/clock/stage"
LOCAL_PUBLISH_DIR="../publish"


# Clear the screen
clear

# Build the production
./prod-build.sh linux-arm64

# Ensure build is ready
if [ ! -d "$LOCAL_PUBLISH_DIR" ]; then
    echo "Error: Build directories not found. Ensure 'dotnet publish' has been run."
    exit 1
fi

#check if the target directory exists and create it if it doesn't
echo "Checking target directory..."
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "[ -d $TARGET_DIR ]"
if [ $? -ne 0 ]; then
    echo "Creating target directory..."
    ssh $TARGET_USER@$TARGET_HOST "mkdir -p $TARGET_DIR"
else
    echo "Target directory exists."
fi

#check if the stage directory exists and create it if it doesn't
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "[ -d $STAGE_DIR ]"
if [ $? -ne 0 ]; then
    echo "Creating stage directory..."
    ssh $TARGET_USER@$TARGET_HOST "mkdir -p $STAGE_DIR"
fi

# Transfer files
echo "Transferring files to the server..."
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "mkdir -p $STAGE_DIR"
scp -i $SSH_KEY -r $LOCAL_PUBLISH_DIR/* $TARGET_USER@$TARGET_HOST:$STAGE_DIR/
echo ""

# Stop existing service
echo "Stopping service..."
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "sudo systemctl stop clock.service || true"
echo ""

# Replace old binaries with new ones
echo "Replacing binaries..."
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "rm -rf $TARGET_DIR"
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "mkdir -p $TARGET_DIR"
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "cp -r $STAGE_DIR/* $TARGET_DIR/"
echo ""

# Start the service
echo "Starting service..."
ssh -i $SSH_KEY $TARGET_USER@$TARGET_HOST "sudo systemctl start clock.service"

echo "Deployment completed successfully."
