using System.Runtime.CompilerServices;
using Amazon.DynamoDBv2.Model;

namespace Villains.Lambda.Api.Services;

public class VillainsService
{
    private readonly IAmazonDynamoDB _dynamoDbClient;
    
    public VillainsService(IAmazonDynamoDB dynamoDbClient)
    {
        _dynamoDbClient = dynamoDbClient;
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
            return Result.Fail(new ExceptionalError(new KeyNotFoundException()));

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
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newVillain"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The ID of the new Villain</returns>
    public async Task<string> CreateAsync(NewVillain newVillain, CancellationToken cancellationToken = default)
    {
        var id = ObjectIdGenerator.New();
        
        var request = new PutItemRequest
        {
            TableName = "villains",
            Item = new Dictionary<string, AttributeValue>
            {
                { "id", new AttributeValue {S = id} },
                { "name", new AttributeValue {S = newVillain.Name}},
                { "powers", new AttributeValue {S = newVillain.Powers}},
                { "imageName", new AttributeValue {S = newVillain.ImageName}},
                { "buttonText", new AttributeValue {S = newVillain.ButtonText}},
                { "saying", new AttributeValue {S = newVillain.Saying }}
            }
        };
        
        await _dynamoDbClient.PutItemAsync(request, cancellationToken);
        return id;
    }
    
    public async Task<Result> UpdateAsync(Villain villain, CancellationToken cancellationToken = default)
    {
        var request = new UpdateItemRequest
        {
            TableName = "villains",
            Key = new()
            {
                ["id"] = new AttributeValue { S = villain.Id }
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":n", new AttributeValue { S = villain.Name }},
                {":p", new AttributeValue { S = villain.Powers }},
                {":i", new AttributeValue { S = villain.ImageName }},
                {":b", new AttributeValue { S = villain.ButtonText }},
                {":s", new AttributeValue { S = villain.Saying }}
            },
            UpdateExpression = "SET #n = :n, #p = :p, #i = :i, #b = :b, #s = :s",
            ExpressionAttributeNames = new Dictionary<string, string>
            {
                {"#n", "name"},
                {"#p", "powers"},
                {"#i", "imageName"},
                {"#b", "buttonText"},
                {"#s", "saying"}
            }
        };
        
        await _dynamoDbClient.UpdateItemAsync(request, cancellationToken);
        return Result.Ok();
    }
}