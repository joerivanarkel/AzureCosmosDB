using CosmosDB;
using UserSecrets;
using Xunit;

namespace CosmosDBTest;

public class CosmosDbRepositoryTest
{
    [Fact]
    public void ShouldCreateItem()
    {
        // Arrange
        string databaseId = "testdb";
        string containerId = "testcontainer";
        string connectionString = UserSecrets<CosmosDbRepositoryTest>.GetSecret("connectionstring");

        // Act
        CosmosDbRepository cosmosDbRepository = new CosmosDbRepository(databaseId, containerId, connectionString);
        Entity entity = new Entity { id = "1", Body = "Hello World!" };
        var result = cosmosDbRepository.CreateItem(entity);

        // Assert
        Assert.True(result.Result);
    }

    [Fact]
    public void ShouldQueryItem()
    {
        // Arrange
        string databaseId = "testdb";
        string containerId = "testcontainer";
        string connectionString = UserSecrets<CosmosDbRepositoryTest>.GetSecret("connectionstring");

        // Act
        CosmosDbRepository cosmosDbRepository = new CosmosDbRepository(databaseId, containerId, connectionString);
        Entity entity = new Entity { id = "1", Body = "Hello World!" };
        cosmosDbRepository.CreateItem(entity);
        var result = cosmosDbRepository.QueryItem(entity.id);

        // Assert
        Assert.NotNull(result.Result);
    }
}