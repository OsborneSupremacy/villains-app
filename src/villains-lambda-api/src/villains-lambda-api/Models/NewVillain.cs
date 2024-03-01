namespace Villains.Lambda.Api.Models;

public record NewVillain
{
    public required string Name { get; init; }

    public required string Powers { get; init; }

    public required string ImageName { get; init; }

    public required string ButtonText { get; init; }

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