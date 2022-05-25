// See https://aka.ms/new-console-template for more information
using CosmosDB;
using UserSecrets;

Console.WriteLine("---");

string databaseId = "testdb";
string containerId = "testcontainer";
string connectionString = UserSecrets<Program>.GetSecret("connectionstring");

CosmosDbRepository cosmosDbRepository = new CosmosDbRepository(databaseId, containerId, connectionString);

Entity entity = new Entity { id = "1", Body = "Hello World!" };

await cosmosDbRepository.CreateItem(entity);
await cosmosDbRepository.QueryItem(entity.id);
