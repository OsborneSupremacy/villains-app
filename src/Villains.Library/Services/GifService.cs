using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Processing;

namespace Villains.Library.Services;

internal class GifService
{
    private static readonly List<int> PercentOriginals = [100, 90, 80, 70, 60, 50, 40, 30, 20, 10, 5];

    private static readonly GifEncoder Encoder = new();

    public static Result<Stream> Resample(Image imageIn, int maxBytes)
    {
        foreach(var percentOfOriginal in PercentOriginals)
        {
            var outStream = Resize(imageIn, percentOfOriginal);
            if (outStream.Length <= maxBytes)
                return outStream;
        }
        return Result.Fail<Stream>(new ExceptionalError(new ArgumentException("Image too large")));
    }

    private static MemoryStream Resize(Image imageIn, int percentOfOriginal)
    {
        var outStream = new MemoryStream();

        if(percentOfOriginal == 100)
        {
            imageIn.Save(outStream, Encoder);
            return outStream;
        }

        imageIn
            .Clone(x => x.Resize(imageIn.Width * percentOfOriginal, imageIn.Height * percentOfOriginal));
        imageIn.Save(outStream, Encoder);
        return outStream;
    }
}