namespace Villains.Library.Lambda;

public class UploadImage
{
    /// <summary>
    /// Function to upload an image to S3.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var uploadRequest = JsonService.DeserializeDefault<ImageUploadRequest>(request.Body);

        if (uploadRequest == null)
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Headers = CorsHeaderService.GetCorsHeaders()
            };

        var validationResult = await new ImageUploadRequestValidator()
            .ValidateAsync(uploadRequest);

        if (!validationResult.IsValid)
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Headers = CorsHeaderService.GetCorsHeaders(),
                Body = JsonService.SerializeDefault(validationResult.Errors)
            };

        var imageService = new ImageService(new AmazonS3Client());

        var result = await imageService
            .UploadImageAsync(uploadRequest, context, CancellationToken.None);

        if (result.IsSuccess)
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Headers = CorsHeaderService.GetCorsHeaders(),
                Body = JsonService.SerializeDefault(result.Value)
            };

        if (result.HasException<InvalidOperationException>())
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.UnsupportedMediaType,
                Headers = CorsHeaderService.GetCorsHeaders(),
                Body = JsonService.SerializeDefault(result.Errors)
            };

        if (result.HasException<ArgumentException>())
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Headers = CorsHeaderService.GetCorsHeaders(),
                Body = JsonService.SerializeDefault(result.Errors)
            };

        var errorMessage = $"""
                            An unexpected type of exception occurred. Details:

                            {result.Errors}
                            """;

        context.Logger.LogError(errorMessage);

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Headers = CorsHeaderService.GetCorsHeaders()
        };
    }
}