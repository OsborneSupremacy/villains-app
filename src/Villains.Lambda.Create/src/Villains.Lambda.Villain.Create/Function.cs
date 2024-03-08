using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Villains.Library.Messaging;
using Villains.Library.Models;
using Villains.Library.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Villains.Lambda.Villain.Create;

public class Function
{
    public static async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var newVillainRequest = JsonService.DeserializeDefault<NewVillain>(request.Body);

        if (newVillainRequest == null)
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Headers = CorsHeaderService.GetCorsHeaders()
            };

        var validationResult = await new NewVillainValidator()
            .ValidateAsync(newVillainRequest);

        if (!validationResult.IsValid)
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Headers = CorsHeaderService.GetCorsHeaders(),
                Body = JsonService.SerializeDefault(validationResult.Errors)
            };

        var villainService = new VillainsService(new AmazonDynamoDBClient());

        var result = await villainService
            .CreateAsync(newVillainRequest, CancellationToken.None);

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Headers = CorsHeaderService.GetCorsHeaders(),
            Body = JsonService.SerializeDefault(new VillainCreateResponse
            {
                VillainId = result
            })
        };
    }
}