#!/bin/bash

# Variables
RPI_USER="ksk"
RPI_IP="192.168.1.106"
RPI_PATH="~/Projects/DBClock"
LOCAL_PATH="/Users/ksk-work/Projects/RPI/RPIDBClock/Src"

# Sync local project files to RPI
rsync -vv -avz --exclude 'bin/' --exclude 'obj/' $LOCAL_PATH $RPI_USER@$RPI_IP:$RPI_PATH

# SSH into RPI and build & run the project
ssh -t $RPI_USER@$RPI_IP << EOF
  # Navigate to project directory
  cd $RPI_PATH
  cd Src

  # Build and run the project
  dotnet build
  dotnet run --project rdc-svc/rdc-svc.csproj
EOF
