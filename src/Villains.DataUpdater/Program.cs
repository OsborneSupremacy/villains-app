using Villains.DataUpdater.Migrations;

DotNetEnv.Env.Load();

const string message = "Enter [red]confirm[/] to continue upgrade process.";

AnsiConsole.MarkupLine(message);

while (
    !AnsiConsole
        .Prompt(new TextPrompt<string>("Input:"))
        .Equals("confirm", StringComparison.OrdinalIgnoreCase)
    )
    AnsiConsole.MarkupLine(message);

CancellationToken ct = new();

var migration = new AddInsertedOn();
await migration.ExecuteAsync(ct);