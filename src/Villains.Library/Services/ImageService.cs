using Amazon.S3.Model;

namespace Villains.Library.Services;

public class ImageService
{
    private readonly IAmazonS3 _s3Client;

    public const int MaxPayloadSize = 6291556;

    public ImageService(IAmazonS3 s3Client)
    {
        _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
    }

    private readonly HashSet<string> _fileExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".apng",
        ".avif",
        ".gif",
        ".jfif",
        ".jpeg",
        ".jpg",
        ".png",
        ".svg",
        ".webp",
        ".bmp",
        ".tiff"
    };

    public async Task<ImageGetResponse> GetImageAsync(string imageName, CancellationToken ct)
    {
        var request = new GetObjectRequest
        {
            BucketName = "villains-images",
            Key = imageName
        };

        try
        {
            var response = await _s3Client.GetObjectAsync(request, ct);

            if (response.HttpStatusCode != HttpStatusCode.OK)
                return NoImageResponse;

            try
            {
                if (response.ResponseStream.Length > MaxPayloadSize)
                    return NoImageResponse;
            }
            catch (NotSupportedException)
            {
                // this happens when the stream is over a certain size.
                // the image is definitely too large when this happens.
                return NoImageResponse;
            }

            var imgSrc = await response.ResponseStream
                .ToImgSrcAsync(response.Headers.ContentType, ct);

            // ensure that imgSrc is not over 6291556 bytes
            if (imgSrc.Length > MaxPayloadSize)
                return NoImageResponse;

            return new ImageGetResponse
            {
                Exists = true,
                ImageSrc = imgSrc,
                FileName = imageName
            };
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"Could not get image from S3. Note that an `Access Denied` exception may be throw if the image doesn't exist. Exception details: {ex}");
            return NoImageResponse;
        }
    }

    private static ImageGetResponse NoImageResponse => new()
    {
        Exists = false,
        ImageSrc = string.Empty,
        FileName = string.Empty
    };

    public async Task<Result<ImageUploadResponse>> UploadImageAsync(
        ImageUploadRequest imageUploadRequest,
        ILambdaContext context,
        CancellationToken ct
        )
    {
        var ext = Path.GetExtension(imageUploadRequest.FileName);

        if (!_fileExtensions.Contains(ext))
            return Result.Fail(new ExceptionalError(new InvalidOperationException()));

        var newFileName = $"{Guid.NewGuid()}{ext}";

        var imageBytesResult =
            Base64Service.GetBytesFromBase64String(imageUploadRequest.Base64EncodedImage);

        if (imageBytesResult.IsFailed)
        {
            var errorDetails = $"""
                                Could not convert base64 string to byte array.

                                Received Base64EncodedImage:

                                -------BEGIN-----------------------------------------
                                {imageUploadRequest.Base64EncodedImage}
                                -------END-------------------------------------------

                                """;

            context.Logger.LogError(errorDetails);
            return Result.Fail(imageBytesResult.Errors);
        }

        var imageMessage = MakeImageSquareService.Edit(new ImageProcessMessage
        {
            OriginalImageBytes = imageBytesResult.Value,
            Modified = false,
            ModifiedImage = null,
            ModifiedImageStream = null,
            MaxBytes = MaxPayloadSize,
            ImageName = imageUploadRequest.FileName
        });

        if (ext.Equals(".gif"))
            imageMessage = new GifService().Resample(imageMessage);

        var request = new PutObjectRequest
        {
            BucketName = "villains-images",
            Key = newFileName,
            InputStream = imageMessage.ModifiedImageStream
                          ?? new MemoryStream(imageBytesResult.Value)
        };

        await _s3Client.PutObjectAsync(request, ct);
        return new ImageUploadResponse
        {
            FileName = newFileName
        };
    }

}