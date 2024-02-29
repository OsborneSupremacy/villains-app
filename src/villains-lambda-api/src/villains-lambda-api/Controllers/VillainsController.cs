using Microsoft.AspNetCore.Mvc;
using Villains.Lambda.Api.Models;
using Villains.Lambda.Api.Services;

namespace Villains.Lambda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VillainsController
{
    private readonly VillainsService _villainsService;

    public VillainsController(VillainsService villainsService)
    {
        _villainsService = villainsService ?? throw new ArgumentNullException(nameof(villainsService));
    }

    [HttpGet]
    [Route("all")]
    [ProducesResponseType(typeof(List<Villain>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Villain>>> Get(CancellationToken cancellationToken) =>
        await _villainsService
            .GetAllAsync(cancellationToken)
            .ToListAsync(cancellationToken);
}