namespace Villains.Library.Lambda;

public class UpdateVillain
{
    public static async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var villainRequest = JsonService.DeserializeDefault<EditVillain>(request.Body);

        if (villainRequest == null)
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Headers = CorsHeaderService.GetCorsHeaders()
            };

        var validationResult = await new EditVillainValidator()
            .ValidateAsync(villainRequest);

        if (!validationResult.IsValid)
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Body = JsonService.SerializeDefault(validationResult.Errors),
                Headers = CorsHeaderService.GetCorsHeaders()
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