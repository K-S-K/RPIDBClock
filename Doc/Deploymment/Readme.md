# Deployment

There are two different deployment types implemented for the project

## Deployment as source code to the target machine

This purpose can be achieved by the only one bash script.

If you need to deploy Clock as a source code to the target Raspberry PI, you should use the script [deploy-rpi-as-source.sh](../../Src/deploy-rpi-as-source.sh) located at the [Src](../../Src/) directory of the project. This script should be executed without any additional parameters.

It would be necessary to change the following constants in the script due to target environment configuration:

- the IP address of the target Raspberry PI machine.
- the user name of the account at the target machine.
- the target directory at the target machine. This directory is different from the directory of binary deployment intentionally - to separate these directories from each other, to not mix experiments with regular work.

## Deployment as binary

### Process description

The production deployment has several phases:

- Building from the source code to the production directory for the target machine CPU architecture.
- Copying from the production directory of the developer machine or build server machine to the staging directory of the target machine. The staging is applied here to reduce downtime.
- After the copying to the staging directory, old version of the service should be stopped at the target machine, if one is running.
- After old version of service is stopped, the content of binary directory should be cleaned up, and the content of the staging directory should be placed to the binary directory.
- Nuw we're ready to execute the new version of the service.

This algorithm is implemented by set of three scripts.

To execute the process user should just execute [prod-update.sh](../../Src/prod-update.sh).

### Scripts description

For the production deployment we have three bash scripts in the project:

- Building script [prod-build.sh](../../Src/prod-build.sh). This script builds all source code to all binary code. It requires two input parameters:
- - Target Runtime Identifier (RID): linux-x64, osx-arm64, win-x64, etc.
- - Publish directory - the directory where the production binary will be built to.
- Deployment script [prod-deploy.sh](../../Src/prod-deploy.sh). This script copies the ready to be executed production files to the staging directory of target machine. Then, script stops the existing service, if tt is working. Then it clears the binary directory. Then it copies staging directory content to the binary directory. Then it starts the service. This script has one input parameter - the publish directory - the directory where the production binary will be taken from. Also this script contains internal variables which should be changed to meet the particular environment:
- - the IP address of the target Raspberry PI machine.
- - the user name of the account at the target machine.
- - the target directories at the target machine for staging and for execution.
- Updating script [prod-update.sh](../../Src/prod-update.sh). This script sets all necessary parameters and executes two another scripts. It contains two internal parameters which should be adopted for the particular environment:
- - Target Runtime Identifier (RID): linux-x64, osx-arm64, win-x64, etc.
- - Publish directory - the directory where the production binary will be built to.
