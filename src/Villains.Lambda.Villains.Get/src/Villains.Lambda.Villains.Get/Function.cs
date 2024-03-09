using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Villains.Library.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Villains.Lambda.Villains.Get;

public class Function
{
    /// <summary>
    /// Gets all villains from the database.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var villainsService = new VillainsService(new AmazonDynamoDBClient());

        var result = await villainsService
            .GetAllAsync(CancellationToken.None)
            .ToListAsync(CancellationToken.None);

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Headers = CorsHeaderService.GetCorsHeaders(),
            Body = JsonService.SerializeDefault(result)
        };
    }
}