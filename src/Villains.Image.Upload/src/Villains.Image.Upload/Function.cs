using System.Net;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.S3;
using Villains.Library.Messaging;
using Villains.Library.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Villains.Image.Upload;

public class Function
{
    /// <summary>
    /// Function to upload an image to S3.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var uploadRequest = JsonSerializer.Deserialize<ImageUploadRequest>(request.Body);

        if (uploadRequest == null)
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };

        var validator = new ImageUploadRequestValidator();
        var validationResult = await validator.ValidateAsync(uploadRequest);

        if (!validationResult.IsValid)
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Body = JsonSerializer.Serialize(validationResult.Errors)
            };

        var imageService = new ImageService(new AmazonS3Client());

        var result = await imageService
            .UploadImageAsync(uploadRequest, CancellationToken.None);

        return result.IsSuccess switch
        {
            true => new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonSerializer.Serialize(result.Value)
            },
            false => result.HasException<InvalidOperationException>()
                ? new APIGatewayProxyResponse { StatusCode = (int)HttpStatusCode.UnsupportedMediaType }
                : new APIGatewayProxyResponse { StatusCode = (int)HttpStatusCode.InternalServerError }
        };
    }
}