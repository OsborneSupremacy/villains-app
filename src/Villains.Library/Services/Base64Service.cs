namespace Villains.Library.Services;

public static class Base64Service
{
    public static Result<byte[]> GetBytesFromBase64String(string base64String)
    {
        try
        {
            if(!base64String.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
                return Convert.FromBase64String(base64String);

            var data = base64String[(base64String.IndexOf(',') + 1)..];
            return Convert.FromBase64String(data);

        } catch (FormatException ex)
        {
            return Result.Fail(new ExceptionalError(ex));
        }
    }
}