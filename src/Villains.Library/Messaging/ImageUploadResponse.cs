namespace Villains.Library.Messaging;

/// <summary>
/// The response from uploading an image.
/// </summary>
public record ImageUploadResponse
{
    /// <summary>
    /// The name of the file on the server.
    /// </summary>
    public required string FileName { get; init; }
}