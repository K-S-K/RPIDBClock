# This script is used to build the project for production
# The script will stop if any error occurs

# Function to print error message and exit
print_error_and_exit() {
    tput setaf 1
    echo "Error: $1"
    tput setaf 2
    exit 1
}

# Variables
PUBLISH_DIR="../publish"
PRJ_SVC="rdc-svc"
TARGET_RID=$1 # Runtime Identifier (RID): linux-x64, osx-arm64, win-x64, etc.

# Check if TARGET_RID is set
if [ -z "$TARGET_RID" ]; then
    print_error_and_exit "TARGET_RID is not set"
fi

# Remove the publish folder
rm -rf $PUBLISH_DIR

# Build the projects
echo "Building"$PRJ_SVC
dotnet publish ./$PRJ_SVC -r $TARGET_RID -c Release --self-contained false -o $PUBLISH_DIR
echo ""

# If any error or warning occurs, the script will stop
if [ $? -ne 0 ]; then
    print_error_and_exit "Build failed"
else
    echo "Build successful"
echo ""
fi
