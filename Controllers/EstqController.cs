using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/integracao/estoques")]
public class EstqController : ControllerBase
{
    private readonly EstqService _service;

    public EstqController(EstqService service)
    {
        _service = service;
    }

    [HttpPost("sync")]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
    {
        var result = await _service.SincronizarEstoquesAsync(cancellationToken);
        return Ok(result);
    }
}
