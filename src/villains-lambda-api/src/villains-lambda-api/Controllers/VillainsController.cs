
namespace Villains.Lambda.Api.Controllers;

[ApiController]
[Route("api/")]
public class VillainsController
{
    private readonly VillainsService _villainsService;

    public VillainsController(VillainsService villainsService)
    {
        _villainsService = villainsService ?? throw new ArgumentNullException(nameof(villainsService));
    }

    [HttpGet]
    [Route("villains")]
    [ProducesResponseType(typeof(List<Villain>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Villain>>> Get(CancellationToken cancellationToken) =>
        await _villainsService
            .GetAllAsync(cancellationToken)
            .ToListAsync(cancellationToken);
    
    [HttpGet]
    [Route("villain/{id}")]
    [ProducesResponseType(typeof(Villain), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Villain>> Get(string id, CancellationToken cancellationToken)
    {
        var result = await _villainsService.GetAsync(id, cancellationToken);
        return result.IsSuccess switch 
        {
            true => result.Value,
            false => new NotFoundResult()
        };
    }
    
    
}