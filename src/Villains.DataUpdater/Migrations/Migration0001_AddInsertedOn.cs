using Villains.Library.Extensions;

namespace Villains.DataUpdater.Migrations;

public class AddInsertedOn
{
    public async Task ExecuteAsync(CancellationToken ct)
    {
        var amazonDynamoDb = new AmazonDynamoDBClient();
        var tableName = "TABLE_NAME".GetEnvVar<string>();

        var response = await amazonDynamoDb.ScanAsync(new()
        {
            TableName = tableName
        }, ct);

        foreach (var item in response.Items)
        {
            var id = item["id"].S;
            var insertedOn = DateTime.UtcNow.ToString("O");

            await amazonDynamoDb.UpdateItemAsync(new()
            {
                TableName = tableName,
                Key = new()
                {
                    ["id"] = new AttributeValue { S = id }
                },
                UpdateExpression = "SET #insertedOn = :insertedOn",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    ["#insertedOn"] = "insertedOn"
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    [":insertedOn"] = new AttributeValue { S = insertedOn }
                }
            }, ct);

            Console.WriteLine("Updated item with id: {0}", id);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
        }
    }
}