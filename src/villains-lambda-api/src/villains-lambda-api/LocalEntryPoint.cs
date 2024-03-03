namespace Villains.Lambda.Api;

/// <summary>
/// The Main function can be used to run the ASP.NET Core application locally using the Kestrel webserver.
/// </summary>
public static class LocalEntryPoint
{
    /// <summary>
    /// Used to run the ASP.NET Core application locally using the Kestrel webserver.
    /// </summary>
    /// <param name="args"></param>
    public static async Task Main(string[] args) =>
        await CreateHostBuilder(args).Build().RunAsync();

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}