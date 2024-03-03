namespace Villains.Lambda.Api.Controllers;

/// <summary>
/// A controller for managing villains.
/// </summary>
[ApiController]
[Route("api/")]
public class VillainsController : Controller
{
    private readonly IVillainsService _villainsService;

    /// <summary>
    /// Constructor for the villains controller.
    /// </summary>
    /// <param name="villainsService"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public VillainsController(IVillainsService villainsService)
    {
        _villainsService = villainsService ?? throw new ArgumentNullException(nameof(villainsService));
    }

    /// <summary>
    /// List all villains.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("villains")]
    [ProducesResponseType(typeof(List<Villain>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Villain>>> Get(CancellationToken ct) =>
        await _villainsService
            .GetAllAsync(ct)
            .ToListAsync(ct);

    /// <summary>
    /// Get a particular villain by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("villain/{id}")]
    [ProducesResponseType(typeof(Villain), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Villain>> Get([FromRoute]string id, CancellationToken ct)
    {
        var result = await _villainsService.GetAsync(id, ct);
        return result.IsSuccess switch
        {
            true =>  result.Value,
            false => result.HasException<KeyNotFoundException>()
                ? new NotFoundResult()
                : new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }

    /// <summary>
    /// Create a new villain.
    /// </summary>
    /// <param name="newVillain"></param>
    /// <param name="ct"></param>
    /// <param name="validator"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("villain")]
    [ProducesResponseType(typeof(CreatedAtRouteResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Villain>> Create(
        NewVillain newVillain,
        CancellationToken ct,
        [FromServices] IValidator<NewVillain> validator
        )
    {
        var validationResult = await validator.ValidateAsync(newVillain, ct);
        if (!validationResult.IsValid)
            return new BadRequestObjectResult(validationResult.Errors);

        var villainId = await _villainsService.CreateAsync(newVillain, ct);
        var villain = await _villainsService.GetAsync(villainId, ct);
        return new CreatedAtRouteResult("villain", new { id = villainId }, villain);
    }

    /// <summary>
    /// Update a villain.
    /// </summary>
    /// <param name="villain"></param>
    /// <param name="ct"></param>
    /// <param name="validator"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("villain/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Villain villain,
        CancellationToken ct,
        [FromServices] IValidator<Villain> validator
        )
    {
        var validationResult = await validator.ValidateAsync(villain, ct);
        if (!validationResult.IsValid)
            return new BadRequestObjectResult(validationResult.Errors);

        var exists = await _villainsService.ExistsAsync(villain.Id, ct);
        if (!exists)
            return new NotFoundResult();

        var result = await _villainsService.UpdateAsync(villain, ct);

        return result.IsSuccess switch
        {
            true => new NoContentResult(),
            false => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}