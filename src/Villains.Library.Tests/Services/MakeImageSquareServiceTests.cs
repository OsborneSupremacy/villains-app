namespace Villains.Library.Tests.Services;

public class MakeImageSquareServiceTests
{
    private const int MaxPayloadBytes = 6291556;

    [Fact]
    public void Edit_ImageIsSquare_ReturnsUnmodified()
    {
        // arrange
        var originalBytes = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream("Villains.Library.Tests.Resources.square.png")!
            .ToByteArray();

        var request = new ImageProcessMessage
        {
            OriginalImageBytes = originalBytes,
            ModifiedImage = null,
            ModifiedImageStream = null,
            Modified = true,
            MaxBytes = MaxPayloadBytes,
            ImageName = "square.png"
        };

        // act
        var message = MakeImageSquareService.Edit(request);

        // assert
        message.OriginalImageBytes.Should().BeEquivalentTo(originalBytes);
        message.ModifiedImage.Should().BeNull();
        message.Modified.Should().BeFalse();
    }

    [Fact]
    public void Edit_ImageIsNotSquare_ReturnsSquare()
    {
        // arrange
        var originalBytes = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream("Villains.Library.Tests.Resources.rectangle.png")!
            .ToByteArray();

        var request = new ImageProcessMessage
        {
            OriginalImageBytes = originalBytes,
            ModifiedImage = null,
            ModifiedImageStream = null,
            Modified = true,
            MaxBytes = MaxPayloadBytes,
            ImageName = "square.png"
        };

        // act
        var message = MakeImageSquareService.Edit(request);

        // assert
        message.Modified.Should().BeTrue();
        message.ModifiedImage.Should().NotBeNull();
        message.ModifiedImage!.Height.Should().Be(message.ModifiedImage.Width);
    }
}