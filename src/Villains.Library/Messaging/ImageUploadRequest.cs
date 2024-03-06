namespace Villains.Library.Messaging;

/// <summary>
/// A request to upload an image.
/// </summary>
public record ImageUploadRequest
{
    /// <summary>
    /// The name of the image file.
    /// </summary>
    public required string FileName { get; init; }

    /// <summary>
    /// The base64 encoded image file.
    /// </summary>
    public required string Base64EncodedImage { get; init; }
}

public class ImageUploadRequestValidator : AbstractValidator<ImageUploadRequest>
{
    public ImageUploadRequestValidator()
    {
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.Base64EncodedImage).NotEmpty();
    }
}
