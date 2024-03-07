namespace Villains.Library.Messaging;

public record VillainCreateResponse
{
    public required string VillainId { get; init; }
}