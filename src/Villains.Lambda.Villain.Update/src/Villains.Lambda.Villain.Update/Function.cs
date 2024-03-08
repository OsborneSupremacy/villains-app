using System.Net;
using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Villains.Library.Models;
using Villains.Library.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Villains.Lambda.Villain.Update;

public class Function
{
    public static async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var villainRequest = JsonService.DeserializeDefault<EditVillain>(request.Body);

        if (villainRequest == null)
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };

        var validationResult = await new EditVillainValidator()
            .ValidateAsync(villainRequest);

        if (!validationResult.IsValid)
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Body = JsonService.SerializeDefault(validationResult.Errors)
            };

        var villainService = new VillainsService(new AmazonDynamoDBClient());

        if (!await villainService.ExistsAsync(villainRequest.Id, CancellationToken.None))
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Headers = CorsHeaderService.GetCorsHeaders()
            };

        await villainService.UpdateAsync(villainRequest, CancellationToken.None);

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Headers = CorsHeaderService.GetCorsHeaders()
        };
    }
}