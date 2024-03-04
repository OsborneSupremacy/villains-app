namespace Villains.Library.Messaging;

/// <summary>
/// A response containing an image file.
/// </summary>
public record GetImageFileResponse
{
    /// <summary>
    /// The file stream of the image file.
    /// </summary>
    public required Stream FileStream { get; init; }

    /// <summary>
    /// The base64 encoded image file.
    /// </summary>
    public required string Base64EncodedImage { get; init; }

    /// <summary>
    /// The MIME type of the image file.
    /// </summary>
    public required string MimeType { get; init; }

    /// <summary>
    /// The name of the image file.
    /// </summary>
    public required string FileName { get; init; }
}