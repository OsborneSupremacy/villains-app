namespace Villains.Lambda.Api.Models;

public record NewVillain
{
    public required string Name { get; set; }

    public required string Powers { get; set; }

    public required string ImageName { get; set; }

    public required string ButtonText { get; set; }

    public required string Saying { get; set; }
}