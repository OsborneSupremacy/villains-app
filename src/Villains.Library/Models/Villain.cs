namespace Villains.Library.Models;

/// <summary>
/// A representation of a villain.
/// </summary>
public record Villain
{
    /// <summary>
    /// The unique identifier of the villain.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// The name of the villain.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The powers of the villain.
    /// </summary>
    public required string Powers { get; init; }

    /// <summary>
    /// The name of the image file associated with the villain.
    /// </summary>
    public required string ImageName { get; init; }

    /// <summary>
    /// The MIME type of the image file.
    /// </summary>
    public required string MimeType { get; init; }

    /// <summary>
    /// The text to display on the button.
    /// </summary>
    public required string ButtonText { get; init; }

    /// <summary>
    /// The saying of the villain.
    /// </summary>
    public required string Saying { get; init; }

    /// <summary>
    /// The date and time the villain was inserted.
    /// </summary>
    public required DateTimeOffset InsertedOn { get; init; }
}

public class VillainValidator : AbstractValidator<Villain>
{
    public VillainValidator()
    {
        RuleFor(villain => villain.Id).Matches("^[0-9a-fA-F]{24}$");
        RuleFor(villain => villain.Name).NotEmpty();
        RuleFor(villain => villain.Powers).NotEmpty();
        RuleFor(villain => villain.ImageName).NotEmpty();
        RuleFor(villain => villain.ButtonText).NotEmpty();
        RuleFor(villain => villain.Saying).NotEmpty();
        RuleFor(villain => villain.InsertedOn).GreaterThanOrEqualTo(DateTimeOffset.UnixEpoch);
    }
}