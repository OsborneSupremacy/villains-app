using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Http;
using Villains.Library.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Villains.Lambda.Villain.Get;

public class Function
{
    public static async Task<APIGatewayProxyResponse> FunctionHandler(
        APIGatewayProxyRequest request,
        ILambdaContext context
        )
    {
        var villainId = request.QueryStringParameters["id"];

        var result = await new VillainsService(new AmazonDynamoDBClient())
            .GetAsync(villainId, CancellationToken.None);

        if(!result.IsSuccess)
            return new APIGatewayProxyResponse
            {
                StatusCode = StatusCodes.Status404NotFound
            };

        return new APIGatewayProxyResponse
        {
            StatusCode = StatusCodes.Status200OK,
            Body = JsonSerializer.Serialize(result.Value)
        };
    }
}