namespace Villains.Library.Messaging;

internal record ImageProcessResponse
{
    public required string FileName { get; init; }

    public required Result<Stream> ImageStream { get; init; }
}