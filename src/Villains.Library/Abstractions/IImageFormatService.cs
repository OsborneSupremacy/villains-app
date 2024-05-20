namespace Villains.Library.Abstractions;

internal interface IImageFormatService
{
    ImageProcessMessage Resample(ImageProcessMessage request);
}