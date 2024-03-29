namespace Villains.Library.Aws.Tests.TestFixtures;


public class EnvironmentFixture : IDisposable
{
    public EnvironmentFixture()
    {
        DotNetEnv.Env.Load();
    }

    public void Dispose()
    {

    }
}