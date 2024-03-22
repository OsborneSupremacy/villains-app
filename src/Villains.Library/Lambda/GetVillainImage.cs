namespace Villains.Library.Lambda;

public class GetVillainImage
{
    /// <summary>
    /// Retrieves an image from S3 and returns it as a base64-encoded string.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var imageName = request.QueryStringParameters["imageName"];

        var imageService = new ImageService(new AmazonS3Client());

        var result = await imageService.GetImageAsync(imageName, CancellationToken.None);

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Headers = CorsHeaderService.GetCorsHeaders(),
            Body = JsonService.SerializeDefault(result),
        };
    }
}