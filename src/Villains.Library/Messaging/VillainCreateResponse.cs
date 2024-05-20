namespace Villains.Library.Messaging;

internal record VillainCreateResponse
{
    /// <summary>
    /// The ID of the newly-created villain.
    /// </summary>
    public required string VillainId { get; init; }
}