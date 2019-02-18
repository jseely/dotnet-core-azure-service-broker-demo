# Product Inventory Service (SQLDB Service Broker Demo)

This repository contains a Pivotal Cloud Foundry microservice demonstrating how to use Azure Service Broker to connect to a Azure SQL DB.

## Deploying

To deploy this application you must have access to a Pivotal Cloud Foundry instance with Azure Service Broker correctly configured.

1. Download and install the [Cloud Formation Client](https://docs.cloudfoundry.org/cf-cli/install-go-cli.html)
1. In the Terminal, type in `$> cf login -a <PCF API url>` and provide your credentials. Now you are connected to Pivotal Cloud Foundry.
1. Verify there is an `azure-sqldb` service in the marketplace `$> cf marketplace -s azure-sqldb`
1. Deploy a standard instance of SQL DB `$> cf create-service azure-sqldb basic mysqldb -c sqldb-config.json`
1. Wait for the service to finish deploying, check `$> cf service mysqldb` for details
1. Update the application name in the `manifest.json` and `$> cf push` the application
1. Test out the api using the following `curl` commands
    1. Get the inventory of the product with id `test`. `$> curl -kv <your-route>/api/inventory/test` should return `HTTP/1.1 404 Not Found`
    1. Set the inventory of the product with id `test`. `$> curl -kH "Content-Type: application/json" -XPUT -d '5' <your-route>/api/inventory/test` should return a body with the json document `{"id": "test", "quantity": 5}`
    1. Verify the inventory has been updated. `$> curl -k <your-route>/api/inventory/test` should return the inventory record for `test`: `{"id": "test", "quantity": 5}`.