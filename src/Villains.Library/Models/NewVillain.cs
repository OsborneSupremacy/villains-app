namespace Villains.Library.Models;

/// <summary>
/// A representation of a new villain.
/// </summary>
public record NewVillain
{
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
    /// The text to display on the button.
    /// </summary>
    public required string ButtonText { get; init; }

    /// <summary>
    /// The saying of the villain.
    /// </summary>
    public required string Saying { get; init; }
}

public class NewVillainValidator : AbstractValidator<NewVillain>
{
    public NewVillainValidator()
    {
        RuleFor(villain => villain.Name).NotEmpty();
        RuleFor(villain => villain.Powers).NotEmpty();
        RuleFor(villain => villain.ImageName).NotEmpty();
        RuleFor(villain => villain.ButtonText).NotEmpty();
        RuleFor(villain => villain.Saying).NotEmpty();
    }
}