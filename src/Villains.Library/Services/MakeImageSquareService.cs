using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Villains.Library.Services;

internal static class MakeImageSquareService
{
    public static ImageProcessMessage Edit(ImageProcessMessage request)
    {
        var imageOut = Image.Load(request.OriginalImageBytes);

        if (imageOut.Width == imageOut.Height)
            return request with { Modified = false, ModifiedImage = null };

        var longerEdge = imageOut.Width > imageOut.Height
            ? imageOut.Width
            : imageOut.Height;

        imageOut.Mutate(x =>
            x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.BoxPad,
                Size = new Size(longerEdge, longerEdge),
            }));

        return request with
        {
            OriginalImageBytes = null,
            Modified = true,
            ModifiedImage = imageOut
        };
    }
}