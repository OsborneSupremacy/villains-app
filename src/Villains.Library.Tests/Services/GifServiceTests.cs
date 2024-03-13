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
        var message = new GifService()
            .Resample(new ImageProcessMessage
            {
                OriginalImageBytes = null,
                ModifiedImage = image,
                ModifiedImageStream = null,
                Modified = true,
                MaxBytes = ImageService.MaxPayloadSize,
                ImageName = "metal-sonic.gif"
            });

        // assert
        message.ModifiedImageStream.Should().NotBeNull();
        message.ModifiedImageStream!.Length.Should().BeLessThan(ImageService.MaxPayloadSize);
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
        var message = new GifService()
            .Resample(new ImageProcessMessage
            {
                OriginalImageBytes = null,
                ModifiedImage = image,
                ModifiedImageStream = null,
                Modified = true,
                MaxBytes = ImageService.MaxPayloadSize,
                ImageName = "shadow.gif"
            });

        // assert
        message.ModifiedImageStream.Should().NotBeNull();
        message.ModifiedImageStream!.Length.Should().BeLessThan(ImageService.MaxPayloadSize);
    }
}