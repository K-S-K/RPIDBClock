# This script is used to build the project for production
# The script will stop if any error occurs


# Variables
PUBLISH_DIR="../publish"  # Local publish directory
PRODUCTION_RID="linux-arm64"  # Runtime Identifier (RID): linux-x64, osx-arm64, win-x64, etc.


# Exit on error
set -e

# Clear the screen
clear

# Display the welcome message
echo "Welcome to the RPI-DB-Clock deployment script."
echo ""

# Build the production
./prod-build.sh $PRODUCTION_RID $PUBLISH_DIR

# Deploy the production
./prod-deploy.sh $PUBLISH_DIR

echo "RPI-DB-Clock updated successfully."
echo ""
