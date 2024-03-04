using Microsoft.Net.Http.Headers;

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
    /// Gets the base64-encoded image by name.
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
    
    /// <summary>
    /// Upload an image.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("image")]
    [RequestSizeLimit(20000000)]
    [ProducesResponseType(typeof(ActionResult<UploadImageResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<UploadImageResponse>> UploadAsync([FromForm]IFormFile? image, CancellationToken ct)
    {
        if (image is null)
            return new BadRequestResult();

        var result = await _imageService.UploadImageAsync(image, ct);

        return result.IsSuccess switch
        {
            true => new OkObjectResult(result.Value),
            false => result.HasException<InvalidOperationException>()
                ? new StatusCodeResult(StatusCodes.Status415UnsupportedMediaType)
                : new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}