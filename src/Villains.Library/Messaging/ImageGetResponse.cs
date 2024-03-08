namespace Villains.Library.Messaging;

/// <summary>
/// A response containing an image file.
/// </summary>
public record ImageGetResponse
{
    /// <summary>
    /// The base64 encoded image file, including the `data:image` prefix (with mime type), so that it can be used as an image src value.
    /// </summary>
    public required string ImageSrc { get; init; }

    /// <summary>
    /// The name of the image file.
    /// </summary>
    public required string FileName { get; init; }
}