namespace Villains.Library.Lambda;

public class GetVillains
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