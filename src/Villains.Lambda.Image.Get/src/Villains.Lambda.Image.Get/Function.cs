using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.S3;
using Villains.Library.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Villains.Lambda.Image.Get;

public class Function
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
            IsBase64Encoded = true,
            Body = result.Base64EncodedImage,
        };
    }
}