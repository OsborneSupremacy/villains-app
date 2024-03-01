namespace Villains.Lambda.Api.Controllers;

[ApiController]
[Route("api/")]
public class ImageController : Controller
{
    private readonly ImageService _imageService;
    
    public ImageController(ImageService imageService)
    {
        _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
    }
    
    [HttpGet]
    [Route("image/{imageName}")]
    public async Task<FileStreamResult> Get([FromRoute]string imageName, CancellationToken ct)
    {
        var result = await _imageService.GetImageAsync(imageName);
        return File(result.Stream, $"image/{result.Extension}");
    }
    
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