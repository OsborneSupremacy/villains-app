using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace Villains.Library.Services;

internal static class PngService
{
    private static readonly List<PngCompressionLevel> CompressionLevels =
    [
        PngCompressionLevel.DefaultCompression,
        PngCompressionLevel.Level7,
        PngCompressionLevel.Level8,
        PngCompressionLevel.BestCompression
    ];

    public static Result<Stream> Resample(Image imageIn, int maxBytes)
    {
        foreach(var compressionLevel in CompressionLevels)
        {
            var outStream = Compress(imageIn, compressionLevel);
            if(outStream.Length <= maxBytes)
                return outStream;
        }
        return Result.Fail<Stream>(new ExceptionalError(new ArgumentException("Image too large")));
    }

    private static MemoryStream Compress(Image imageIn, PngCompressionLevel compressionLevel)
    {
        var outStream = new MemoryStream();
        imageIn
            .Save(outStream,
                new PngEncoder { CompressionLevel = compressionLevel }
            );
        return outStream;
    }
}