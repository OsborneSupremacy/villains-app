namespace Villains.Library.Lambda;

public class GetVillain
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
                StatusCode = (int)HttpStatusCode.NotFound
            };

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Headers = CorsHeaderService.GetCorsHeaders(),
            Body = JsonService.SerializeDefault(result.Value)
        };
    }
}