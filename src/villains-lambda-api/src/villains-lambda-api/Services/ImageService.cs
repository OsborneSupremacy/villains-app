using System.Net;
using System.Reflection;
using Amazon.S3.Model;

namespace Villains.Lambda.Api.Services;

internal class ImageService : IImageService
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
                MimeType = response.Headers.ContentType,
                FileName = imageName
            };
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return await GetNotFoundImageAsync(ct);
        }
    }

    private Task<GetImageFileResponse> GetNotFoundImageAsync(CancellationToken ct)
    {
        var stream = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream("Villains.Lambda.Api.Resources.notfound.jfif");

        return Task.FromResult(new GetImageFileResponse
        {
            FileStream = stream!,
            MimeType = "image/jpeg",
            FileName = "notfound.jfif"
        });
    }

    public async Task<Result<UploadImageResponse>> UploadImageAsync(IFormFile image, CancellationToken ct)
    {
        var ext = Path.GetExtension(image.FileName);

        if (!_fileExtensions.Contains(ext))
            return Result.Fail(new ExceptionalError(new InvalidOperationException()));

        var newFileName = Guid.NewGuid() + ext;

        var request = new PutObjectRequest
        {
            BucketName = "villains-images",
            Key = newFileName,
            InputStream = image.OpenReadStream()
        };

        await _s3Client.PutObjectAsync(request, ct);
        return new UploadImageResponse
        {
            FileName = newFileName
        };
    }
}