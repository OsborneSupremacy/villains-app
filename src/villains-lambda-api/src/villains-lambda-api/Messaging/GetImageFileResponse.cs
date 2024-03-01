namespace Villains.Lambda.Api.Messaging;

public record GetImageFileResponse
{
    public required Stream Stream { get; init; }
    
    public required string Extension { get; init; }
}   