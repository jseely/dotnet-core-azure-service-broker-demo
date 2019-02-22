# Product Image Service (Azure Storage Service Broker Demo)

This repository contains a Pivotal Cloud Foundry microservice demonstrating how to use Azure Service Broker to connect to a Cosmos DB.

## Deploying

To deploy this application you must have access to a Pivotal Cloud Foundry instance with Azure Service Broker correctly configured.

1. Download and install the [Cloud Formation Client](https://docs.cloudfoundry.org/cf-cli/install-go-cli.html)
1. In the Terminal, type in `$> cf login -a <PCF API url>` and provide your credentials. Now you are connected to Pivotal Cloud Foundry.
1. Verify there is an `azure-storage` service in the marketplace `$> cf marketplace -s azure-storage`
1. Deploy a standard tier of Azure Storage `$> cf create-service azure-storage standard myazurestorage -c azure-storage-config.json`
1. Wait for the service to finish deploying, check `$> cf service myazurestorage` for details
1. Update the application name in the `manifest.json` and `$> cf push` the application
1. Test out the api using the following `curl` commands
    1. Upload a file (image or otherwise). `$> curl -XPOST -F "file=@<local-path-to-file>" <your-route>/api/image/<remote-directory>` should return HTTP 200 with the body `Uploaded file '<filename>' to path '<remote-directory>/<filename>'.
    1. Download the file. `$> curl <your-route>/api/image/<remote-directory>/<filename> > <local-filename-for-download>`
    1. Validate the downloaded file matches the uploaded file.