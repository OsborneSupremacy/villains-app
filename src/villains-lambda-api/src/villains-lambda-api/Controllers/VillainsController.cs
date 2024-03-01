
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
            true =>  result.Value,
            false => result.HasException<KeyNotFoundException>()
                ? new NotFoundResult()
                : new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
    
    [HttpPost]
    [Route("villain")]
    [ProducesResponseType(typeof(CreatedAtRouteResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Villain>> Create(NewVillain newVillain, CancellationToken cancellationToken)
    {
        var villainId = await _villainsService.CreateAsync(newVillain, cancellationToken);
        var villain = await _villainsService.GetAsync(villainId, cancellationToken);
        return new CreatedAtRouteResult("villain", new { id = villainId }, villain);
    }
    
    [HttpPut]
    [Route("villain/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Villain villain, CancellationToken cancellationToken)
    {
        var current = await _villainsService.GetAsync(villain.Id, cancellationToken);
        if (!current.IsSuccess)
            return new NotFoundResult();
        
        var result = await _villainsService.UpdateAsync(villain, cancellationToken);

        return result.IsSuccess switch
        {
            true => new NoContentResult(),
            false => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}