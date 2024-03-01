namespace Villains.Lambda.Api.Services;

public static class ObjectIdGenerator
{
    public static string New()
    {
        // Create a Random instance
        Random random = new();

        // Generate 12 random bytes
        byte[] randomBytes = new byte[12];
        random.NextBytes(randomBytes);

        // Convert the bytes to a hexadecimal string
        return BitConverter.ToString(randomBytes).Replace("-", string.Empty);
    }
}