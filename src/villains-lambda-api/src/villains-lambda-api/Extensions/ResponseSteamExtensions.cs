namespace Villains.Lambda.Api.Extensions;

internal static class ResponseSteamExtensions
{
    public static async Task<byte[]> ToByteArrayAsync(this Stream stream, CancellationToken ct)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, ct);
        return memoryStream.ToArray();
    }

    public static async Task<string> ToBase64StringAsync(this Stream stream, CancellationToken ct) =>
        Convert.ToBase64String(await ToByteArrayAsync(stream, ct));
}