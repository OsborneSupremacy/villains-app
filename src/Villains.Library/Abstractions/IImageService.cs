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
    Task<ImageGetResponse> GetImageAsync(string imageName, CancellationToken ct);
    /// <summary>
    /// Uploads an image to the image store.
    /// </summary>
    /// <param name="imageUploadRequest"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<Result<ImageUploadResponse>> UploadImageAsync(ImageUploadRequest imageUploadRequest, CancellationToken ct);
}