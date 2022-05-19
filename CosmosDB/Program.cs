// See https://aka.ms/new-console-template for more information
using System.Net;
using CosmosDB;
using Microsoft.Azure.Cosmos;

Console.WriteLine("Hello, World!");

CosmosClient cosmosClient;
Database database;
Container container;

string databaseId = "testdb";
string containerId = "testcontainer";
string connectionString = "AccountEndpoint=https://cosmosdbjoeritest.documents.azure.com:443/;AccountKey=OJOWAb7gTiF4hgz862zy9m19qlYSkm3Imi6TeqmJZRrvB085MmOfUgCyzDdE9FfEIyQ9OAcbvHVO1mlYZyKcDQ==;";

cosmosClient = new CosmosClient(connectionString);

database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
Console.WriteLine("Created Database: {0}\n", database.Id);

container = await database.CreateContainerIfNotExistsAsync(containerId, "/id");
Console.WriteLine("Created Container: {0}\n", container.Id);

Entity entity = new Entity { Id = "1", Body = "Hello World" };
try
{
    // Read the item to see if it exists.  
    ItemResponse<Entity> entityResponse = await container.ReadItemAsync<Entity>(entity.Id, new PartitionKey(entity.Id));
    Console.WriteLine("Item in database with id: {0} already exists\n", entityResponse.Resource.Id);
}
catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
{
    // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
    ItemResponse<Entity> entityResponse = await container.CreateItemAsync<Entity>(entity, new PartitionKey(entity.Id));

    // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
    Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", entityResponse.Resource.Id, entityResponse.RequestCharge);
}