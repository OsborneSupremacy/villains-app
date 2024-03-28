using Villains.Library.Extensions;

DotNetEnv.Env.Load();

const string message = "Enter [red]confirm[/] to continue upgrade process.";

AnsiConsole.MarkupLine(message);

while (
    !AnsiConsole
        .Prompt(new TextPrompt<string>("Input:"))
        .Equals("confirm", StringComparison.OrdinalIgnoreCase)
    )
    AnsiConsole.MarkupLine(message);

AnsiConsole.WriteLine("TABLE_NAME".GetEnvVar<string>());
AnsiConsole.Write("HELLO WORLD!");