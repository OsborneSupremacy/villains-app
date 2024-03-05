namespace Villains.Library.Extensions;

public static class StringExtensions
{
    private static readonly ILookup<string, string> ExtensionMimeTypeMap = new Dictionary<string, string>
    {
        { ".apng", "image/apng" },
        { ".avif", "image/avif" },
        { ".gif", "image/gif" },
        { ".jfif", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".jpg", "image/jpeg" },
        { ".png", "image/png" },
        { ".svg", "image/svg+xml" },
        { ".webp", "image/webp" },
        { ".bmp", "image/bmp" },
        { ".tiff", "image/tiff" }
    }.ToLookup(kvp => kvp.Key, kvp => kvp.Value);

    public static string GetMimeType(this string fileName)
    {
        var extension = Path.GetExtension(fileName);
        return ExtensionMimeTypeMap[extension].FirstOrDefault() ?? "image/jpeg";
    }
}