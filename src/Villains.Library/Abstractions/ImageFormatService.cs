namespace Villains.Library.Abstractions;

internal interface ImageFormatService
{
    ImageProcessMessage Resample(ImageProcessMessage request);
}