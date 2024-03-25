namespace Villains.Library.Extensions;

public static class EnvVarExtensions
{
    public static T GetEnvVar<T>(this string key)
    {
        var value = Environment.GetEnvironmentVariable(key);

        if (value == null)
            throw new ArgumentNullException(key);

        return (T)Convert.ChangeType(value, typeof(T));
    }
}