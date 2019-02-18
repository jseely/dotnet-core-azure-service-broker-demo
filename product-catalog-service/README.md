# Product Catalog Service (CosmosDB Service Broker Demo)

This repository contains a Pivotal Cloud Foundry microservice demonstrating how to use Azure Service Broker to connect to a Cosmos DB.

## Deploying

To deploy this application you must have access to a Pivotal Cloud Foundry instance with Azure Service Broker correctly configured.

1. Download and install the [Cloud Formation Client](https://docs.cloudfoundry.org/cf-cli/install-go-cli.html)
1. In the Terminal, type in `$> cf login -a <PCF API url>` and provide your credentials. Now you are connected to Pivotal Cloud Foundry.
1. Verify there is an `azure-cosmosdb` service in the marketplace `$> cf marketplace -s azure-cosmosdb`
1. Deploy a standard instance of Cosmos DB `$> cf create-service azure-cosmosdb standard mycosmosdb -c cosmosdb-config.json`
1. Wait for the service to finish deploying, check `$> cf service mycosmosdb` for details
1. Update the application name in the `manifest.json` and `$> cf push` the application
1. Test out the api using the following `curl` commands
    1. Get the list of products in the catalog, it should be an empty array at this point. `$> curl <your-route>/api/products` should return `[]`
    1. Push a product to the catalog. `$> curl -H "Content-Type: application/json" -XPOST -d '{"id": "test", "name": "test", "description": "test"}' <your-route>/api/products`
    1. Verify that your product has been added to the catalog. `$> curl <your-route>/api/products` should return an array containing your product.