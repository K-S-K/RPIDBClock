#!/bin/bash

# Variables
RPI_USER="ksk"
RPI_IP="192.168.1.106"
RPI_PATH="~/Projects/DBClock"
# LOCAL_PATH="/Users/ksk-work/Projects/RPI/RPIDBClock/Src"

# Get the local path dynamically
LOCAL_PATH="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

# Display the welcome message
echo ""
echo ""
echo "Welcome to the RPI-DB-Clock deployment as source code procedure."
echo "The source code will be synced to the Raspberry Pi at" $RPI_IP
echo "from the local path at the source machine:" $LOCAL_PATH
echo "to the path at the target machine:" $RPI_PATH
echo "The target path will be created if it does not exist."
echo ""
echo "After syncing, the script will SSH into the Raspberry Pi to"
echo "build it there, and run the application."
echo ""

# Let user to decide to continue or not
read -p "Do you want to continue? (y/n): " choice
if [[ "$choice" != "y" && "$choice" != "Y" ]]; then
    echo "Deployment cancelled."
    echo ""
    exit 0
fi

# Sync local project files to RPI
rsync -vv -avz --exclude 'bin/' --exclude 'obj/' $LOCAL_PATH $RPI_USER@$RPI_IP:$RPI_PATH

# SSH into RPI and build & run the project
ssh -t $RPI_USER@$RPI_IP << EOF
  # Check if the project directory exists, if not create it
  if [ ! -d "$RPI_PATH" ]; then
    mkdir -p $RPI_PATH
  fi

  # Navigate to project directory
  cd $RPI_PATH
  cd Src

  # Build and the project
  dotnet build

  # Run the project
  # dotnet run --project rdc-svc/rdc-svc.csproj
EOF
