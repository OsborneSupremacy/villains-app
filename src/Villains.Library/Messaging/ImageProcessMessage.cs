using SixLabors.ImageSharp;

namespace Villains.Library.Messaging;

/// <summary>
/// I don't like have a record with so many fields, many of them nullable.
/// Image processing is difficult, because the image may or may not change formatting
/// as it flows through the services. We always want to preserve pre-processed version
/// of the image, when it doesn't need to be processed.
/// </summary>
internal record ImageProcessMessage
{
    public required byte[]? OriginalImageBytes { get; init; }

    public required bool Modified { get; init; }

    public required Image? ModifiedImage { get; init; }

    public required Stream? ModifiedImageStream { get; init; }

    public required int MaxBytes { get; init; }

    /// <summary>
    /// The full name of the image, including the extension. Do not include the path.
    /// </summary>
    public required string ImageName { get; init; }
}