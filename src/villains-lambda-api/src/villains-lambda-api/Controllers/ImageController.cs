namespace Villains.Lambda.Api.Controllers;

/// <summary>
/// A controller for managing images.
/// </summary>
[ApiController]
[Route("api/")]
public class ImageController : Controller
{
    private readonly IImageService _imageService;

    /// <summary>
    /// Constructor for the image controller.
    /// </summary>
    /// <param name="imageService"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ImageController(IImageService imageService)
    {
        _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
    }

    /// <summary>
    /// Gets the image by name.
    /// </summary>
    /// <param name="imageName"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("image/{imageName}")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    public async Task<FileStreamResult> Get([FromRoute]string imageName, CancellationToken ct)
    {
        var result = await _imageService.GetImageAsync(imageName, ct);
        return File(result.FileStream, result.MimeType);
    }
}