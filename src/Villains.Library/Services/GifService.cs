using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Processing;
using Villains.Library.Abstractions;

namespace Villains.Library.Services;

internal class GifService : IImageFormatService
{
    private static readonly List<int> PercentOriginals = [100, 90, 80, 70, 60, 50, 40, 30, 20, 10, 5];

    private static readonly GifEncoder Encoder = new();

    public ImageProcessMessage Resample(ImageProcessMessage request)
    {
        if(!request.Modified && request.OriginalImageBytes!.Length <= request.MaxBytes)
            return request;

        var image = request.ModifiedImage ?? Image.Load(request.OriginalImageBytes);

        foreach(var percentOfOriginal in PercentOriginals)
        {
            var outStream = Resize(image, percentOfOriginal);
            if (outStream.Length <= request.MaxBytes)
                return request with
                {
                    OriginalImageBytes = null,
                    Modified = true,
                    ModifiedImage = null,
                    ModifiedImageStream = outStream
                };
        }

        throw new AggregateException("Image cannot be resized to fit within the maximum size.");
    }

    private static MemoryStream Resize(Image imageIn, int percentOfOriginal)
    {
        var outStream = new MemoryStream();

        // why are we even doing this? We can't determine the size of the image until it's loaded
        // into a stream, which will change the size since it's a different encoder.
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