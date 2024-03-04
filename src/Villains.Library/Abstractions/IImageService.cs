using Microsoft.AspNetCore.Http;

namespace Villains.Library.Abstractions;

/// <summary>
/// The service for working with images.
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Gets an image from the the image store.
    /// </summary>
    /// <param name="imageName"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<GetImageFileResponse> GetImageAsync(string imageName, CancellationToken ct);

    /// <summary>
    /// Uploads an image to the image store.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<Result<UploadImageResponse>> UploadImageAsync(IFormFile image, CancellationToken ct);
}