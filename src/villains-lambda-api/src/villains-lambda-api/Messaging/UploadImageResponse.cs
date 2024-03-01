namespace Villains.Lambda.Api.Messaging;

/// <summary>
/// <code>FileName</code> is the name of the file on the server.
/// The image will be given a unique name when uploaded.
/// </summary>
public record UploadImageResponse
{
    public required string FileName { get; init; }
}