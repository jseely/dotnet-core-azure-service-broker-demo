# Product Catalog Service (CosmosDB Service Broker Demo)

This repository contains a Pivotal Cloud Foundry microservice demonstrating how to use Azure Service Broker to connect to a Cosmos DB database.

## Deploying

To deploy this application you must have access to a Pivotal Cloud Foundry instance with Azure Service Broker correctly configured.

1. Use your Cloud Formation client to login to your cluster `$> cf login ...`
1. Verify there is an `azure-cosmosdb` service in the marketplace `$> cf marketplace -s azure-cosmosdb`
1. Deploy a standard instance of Cosmos DB `$> cf create-service azure-cosmosdb standard mycosmosdb -c cosmosdb-config.json`
1. Wait for the service to finish deploying, check `$> cf service mycosmosdb` for details
1. Update the application name in the `manifest.json` and `$> cf push` the application
1. Test out the api using `GET <your-route>/api/products`, `POST <your-route>/api/products` with a json body `{"id": "test", "name": "test", "description": "test"`}`