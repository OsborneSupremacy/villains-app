using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Villains.Library.Services;

public static class ImageResizeService
{
    public static Image Edit(byte[] imageIn)
    {
        var imageOut = Image.Load(imageIn);

        if (imageOut.Width == imageOut.Height)
            return imageOut;

        var longerEdge = imageOut.Width > imageOut.Height
            ? imageOut.Width
            : imageOut.Height;

        imageOut.Mutate(x =>
            x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.BoxPad,
                Size = new Size(longerEdge, longerEdge),
            }));

        return imageOut;
    }
}