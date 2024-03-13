using System.Reflection;
using SixLabors.ImageSharp;

namespace Villains.Library.Tests.Services;

public class GifServiceTests
{

    [Fact]
    public async Task Resample_ImageIsUnderMax_Succeeds()
    {
        // arrange
        var imageStream = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream("Villains.Library.Tests.Resources.metal-sonic.gif");

        var image = await Image.LoadAsync(imageStream!);

        // act
        var resampledImageResult = GifService.Resample(image, ImageService.MaxPayloadSize);

        // assert
        resampledImageResult.IsSuccess.Should().BeTrue();
        // base64 changes because Image is encoded to stream, using
        // a Gif encoder not necessarily the same as the original, so the
        // base64 string will most likely be different.
    }

    [Fact]
    public async Task Resample_ImageIsOverMax_Succeeds()
    {
        // arrange
        var imageStream = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream("Villains.Library.Tests.Resources.shadow.gif");

        var image = await Image.LoadAsync(imageStream!);

        // act
        var resampledImageResult = GifService.Resample(image, ImageService.MaxPayloadSize);

        // assert
        resampledImageResult.IsSuccess.Should().BeTrue();
    }
}