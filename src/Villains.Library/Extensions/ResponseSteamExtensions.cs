namespace Villains.Library.Extensions;

internal static class ResponseSteamExtensions
{
    public static async Task<byte[]> ToByteArrayAsync(this Stream stream, CancellationToken ct)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, ct);
        return memoryStream.ToArray();
    }

    public static byte[] ToByteArray(this Stream stream)
    {
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    public static async Task<string> ToBase64StringAsync(this Stream stream, CancellationToken ct)
    {
        stream.Position = 0;
        return Convert.ToBase64String(await ToByteArrayAsync(stream, ct));
    }

    public static async Task<string> ToImgSrcAsync(this Stream stream, string contentType, CancellationToken ct) =>
        $"data:{contentType};base64,{await stream.ToBase64StringAsync(ct)}";
}