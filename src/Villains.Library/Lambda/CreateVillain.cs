namespace Villains.Library.Lambda;

public class CreateVillain
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