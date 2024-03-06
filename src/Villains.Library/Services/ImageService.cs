using System.Net;
using System.Reflection;
using Amazon.S3;
using Amazon.S3.Model;

namespace Villains.Library.Services;

public class ImageService : IImageService
{
    private readonly IAmazonS3 _s3Client;

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

    public async Task<GetImageFileResponse> GetImageAsync(string imageName, CancellationToken ct)
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
                return await GetNotFoundImageAsync(ct);

            return new GetImageFileResponse
            {
                FileStream = response.ResponseStream,
                Base64EncodedImage = await response.ResponseStream.ToBase64StringAsync(ct),
                MimeType = response.Headers.ContentType,
                FileName = imageName
            };
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"Could not get image from S3. Note that an `Access Denied` exception may be throw if the image doesn't exist. Exception details: {ex}");
            return await GetNotFoundImageAsync(ct);
        }
    }

    private async Task<GetImageFileResponse> GetNotFoundImageAsync(CancellationToken ct)
    {
        var stream = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream("Villains.Library.Resources.notfound.jfif");

        return new GetImageFileResponse
        {
            FileStream = stream!,
            Base64EncodedImage = await stream!.ToBase64StringAsync(ct),
            MimeType = "image/jpeg",
            FileName = "notfound.jfif"
        };
    }

    public async Task<Result<ImageUploadResponse>> UploadImageAsync(
        ImageUploadRequest imageUploadRequest,
        CancellationToken ct
        )
    {
        var ext = Path.GetExtension(imageUploadRequest.FileName);

        if (!_fileExtensions.Contains(ext))
            return Result.Fail(new ExceptionalError(new InvalidOperationException()));

        var newFileName = $"{Guid.NewGuid()}{ext}";
        // convert base64 string to

        var request = new PutObjectRequest
        {
            BucketName = "villains-images",
            Key = newFileName,
            InputStream = new MemoryStream(Convert.FromBase64String(imageUploadRequest.Base64EncodedImage))
        };

        await _s3Client.PutObjectAsync(request, ct);
        return new ImageUploadResponse
        {
            FileName = newFileName
        };
    }

}