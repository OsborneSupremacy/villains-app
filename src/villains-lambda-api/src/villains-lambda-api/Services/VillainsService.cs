using System.Runtime.CompilerServices;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Villains.Lambda.Api.Models;

namespace Villains.Lambda.Api.Services;

public class VillainsService
{
    private readonly IAmazonDynamoDB _dynamoDbClient;
    
    private readonly IDynamoDBContext _dbContext;
    
    public VillainsService(IAmazonDynamoDB dynamoDbClient, IDynamoDBContext dbContext)
    {
        _dynamoDbClient = dynamoDbClient;
        _dbContext = dbContext;
    }

    public async IAsyncEnumerable<Villain> GetAllAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
    {
        var response = await _dynamoDbClient.ScanAsync(new()
        {
            TableName = "villains"
        }, cancellationToken);

        foreach (var doc in response.Items.Select(Document.FromAttributeMap))
            yield return _dbContext.FromDocument<Villain>(doc);
    }
}