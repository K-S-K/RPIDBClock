# Deployment

Two different deployment types are implemented for this project.

- Source-code deployment is intended for development and experimentation (requires .NET SDK).
- Binary deployment is intended for stable, repeatable production use (requires .NET Runtime or SDK).

In this project, “production deployment” means running the clock as a long-lived systemd service on the target Raspberry Pi.

**Note:** Before deployment, assume that:

- .NET runtime and/or SDK is installed on the target
- user has permission to manage the service
- systemd is available and configured
- SSH access is configured

## Deployment as source code to the target machine

This purpose can be achieved by a single bash script.

If you need to deploy Clock as source code to the target Raspberry Pi, you should use the script [deploy-rpi-as-source.sh](../../Src/deploy-rpi-as-source.sh) located at the [Src](../../Src/) directory of the project. This script should be executed without any additional parameters.

It would be necessary to change the following constants in the script due to the target environment configuration:

- the IP address of the target Raspberry PI machine.
- the user name of the account at the target machine.
- the target directory on the target machine. This directory is intentionally different from the binary deployment directory to separate them and avoid mixing experiments with regular work.

## Deployment as binary

### Process description

The production deployment has several phases:

- Building from the source code to the production directory for the target machine CPU architecture.
- Copying from the production directory of the developer machine or build server machine to the staging directory of the target machine. **The staging is applied here to reduce downtime**.
- After the copying to the staging directory, the old version of the service is stopped (if running).
- After the old version of the service is stopped, the content of the binary directory is cleaned up, and the content of the staging directory is placed in the binary directory.
- Now we're ready to execute the new version of the service.

This algorithm is implemented using three scripts.

To execute the process, the user should run [prod-update.sh](../../Src/prod-update.sh).

### Scripts description

This section describes the responsibility borders between scripts and some localization details for the particular application environment.

For the production deployment, we have three bash scripts in the project:

- Building script [prod-build.sh](../../Src/prod-build.sh). This script builds all source code into all binary code. It requires two input parameters:
- - Target Runtime Identifier (RID): **linux-arm64** for Raspberry Pi, for other systems it can be linux-x64, osx-arm64, win-x64, etc.
- - Publish directory - the directory where the production binary will be built to.
- Deployment script [prod-deploy.sh](../../Src/prod-deploy.sh). This script copies the ready-to-run production files to the target machine's staging directory. Then, the script stops the existing service if it is working. Then it clears the binary directory. Then it copies the staging directory content to the binary directory. Then it starts the service. This script has one input parameter: the publish directory, the directory from which the production binary will be taken. Also, this script contains internal variables that should be changed to meet the particular environment:
- - the IP address of the target Raspberry PI machine.
- - the user name of the account at the target machine.
- - the target directories at the target machine for staging and for execution.
- Updating script [prod-update.sh](../../Src/prod-update.sh). This script sets all necessary parameters and executes two other scripts. It contains two internal parameters that should be adopted for the particular environment:
- - Target Runtime Identifier (RID): **linux-arm64** for Raspberry Pi, for other systems it can be linux-x64, osx-arm64, win-x64, etc.
- - Publish directory - the directory where the production binary will be built to.

These values are intentionally kept inside the scripts to keep the deployment flow simple and explicit for a single-device setup.
