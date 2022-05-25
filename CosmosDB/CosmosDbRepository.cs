using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CosmosDB;

public class CosmosDbRepository
{
    private CosmosClient _cosmosClient;
    private Database _database;
    private Container _container;

    public CosmosDbRepository(string databaseId, string containerId, string connectionString)
    {
        _cosmosClient = new CosmosClient(connectionString);
        _database = GetDatabase(databaseId, _cosmosClient);
        _container = GetContainer(containerId, _database);
    }

    private Database GetDatabase(string databaseId, CosmosClient cosmosClient)
    {
        Database database = cosmosClient.GetDatabase(databaseId);
        if (database.Equals(null))
        {
            cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        }
        return database;
    }

    private Container GetContainer(string containerId, Database database)
    {
        Container container = database.GetContainer(containerId);
        if (container.Equals(null))
        {
            database.CreateContainerIfNotExistsAsync(containerId, "/id");
        }
        return container;
    }

    public async Task<bool> CreateItem(Entity entity)
    {
        try
        {
            ItemResponse<Entity> entityResponse = await _container.ReadItemAsync<Entity>(entity.id, new PartitionKey(entity.id));
            Console.WriteLine("Item in database with id: {0} already exists\n", entityResponse.Resource.id);
            return true;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            ItemResponse<Entity> entityResponse = await _container.CreateItemAsync<Entity>(entity, new PartitionKey(entity.id));
            Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", entityResponse.Resource.id, entityResponse.RequestCharge);
            return true;
        }
        catch
        {
            Console.WriteLine("Failed to create item in database.\n");
            return false;
        }
    }

    public async Task<Entity> QueryItem(string id)
    {
        var queryable = _container.GetItemLinqQueryable<Entity>();
        var iterator = queryable.Where(p => p.id == id.ToString()).ToFeedIterator();
        if (iterator.HasMoreResults)
        {
            FeedResponse<Entity> response = await iterator.ReadNextAsync();
            if (response.Count != 0)
            {
                foreach (Entity entity in response)
                {
                    Console.WriteLine($"Read item {entity.id} from database");
                    Console.WriteLine($"    Body: {entity.Body}");
                    return entity;
                }
            }
            else
            {
                Console.WriteLine("No item found in database\n");
                return null;
            }
        }
        return null;
    }
}
