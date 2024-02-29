using System.Runtime.CompilerServices;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
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

        foreach (var villain in response.Items.Select(item => new Villain
             {
                 Id = item["id"].S,
                 Name = item["name"].S,
                 Powers = item["powers"].S,
                 ImageName = item["imageName"].S,
                 ButtonText = item["buttonText"].S,
                 Saying = item["saying"].S
             }))
            yield return villain;
    }
    
    public async Task<Result<Villain>> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var response = await _dynamoDbClient.GetItemAsync(new()
        {
            TableName = "villains",
            Key = new()
            {
                ["id"] = new AttributeValue { S = id }
            }
        }, cancellationToken);

        if (response.Item is null)
            return Result.Fail("Not found");

        return new Villain
        {
            Id = response.Item["id"].S,
            Name = response.Item["name"].S,
            Powers = response.Item["powers"].S,
            ImageName = response.Item["imageName"].S,
            ButtonText = response.Item["buttonText"].S,
            Saying = response.Item["saying"].S
        };
    }
}