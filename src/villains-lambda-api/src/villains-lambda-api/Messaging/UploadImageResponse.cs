namespace Villains.Lambda.Api.Messaging;

/// <summary>
/// The response from uploading an image.
/// </summary>
public record UploadImageResponse
{
    /// <summary>
    /// The name of the file on the server.
    /// </summary>
    public required string FileName { get; init; }
}