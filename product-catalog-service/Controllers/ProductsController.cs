using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using product_catalog_service.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace product_catalog_service.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDocumentClient _cosmosClient;
        private readonly Uri _collectionUri;
        private static string CosmosCollectionName = "products";

        public ProductsController(IDocumentClient client)
        {
            _cosmosClient = client;
            try
            {
                _cosmosClient.CreateDatabaseIfNotExistsAsync(new Database{Id = Constants.CosmosDBName}).Wait();
                _cosmosClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(Constants.CosmosDBName), new DocumentCollection{ Id = CosmosCollectionName }).Wait();
                _collectionUri = UriFactory.CreateDocumentCollectionUri(Constants.CosmosDBName, CosmosCollectionName);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error building ProductsController: {ex.ToString()}");
                throw;
            }
        }

        [HttpGet]
        [Route("api/[controller]")]
        public IActionResult ListProducts()
        {
            var query = _cosmosClient.CreateDocumentQuery<Product>(_collectionUri, $"SELECT * FROM {CosmosCollectionName}");
            return Ok(query.ToArray());
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<IActionResult> CreateProduct([FromBody]Product product)
        {
            try
            {
                await _cosmosClient.CreateDocumentAsync(_collectionUri, product);
                return StatusCode(201);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to create a new product with id <{product.Id}> with error: {ex.ToString()}");
                return StatusCode(409);
            }
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            try
            {
                var documentResponse = await _cosmosClient.ReadDocumentAsync<Product>(UriFactory.CreateDocumentUri(Constants.CosmosDBName, CosmosCollectionName, id));
                return Ok(documentResponse.Document);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error getting document with id <{id}>: {ex.ToString()}");
                return StatusCode(501);
            }
        }

        [HttpPut]
        [Route("api/[controller]")]
        public IActionResult UpdateProduct([FromBody]Product product)
        {
            try
            {
                _cosmosClient.UpsertDocumentAsync(_collectionUri, product);
                return StatusCode(200);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to create a new product with id <{product.Id}> with error: {ex.ToString()}");
                return StatusCode(401);
            }
        }
    }
}
