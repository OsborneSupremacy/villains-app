namespace Villains.Lambda.Api.Messaging;

public record UploadImageResponse
{
    public required string FileName { get; init; }
}