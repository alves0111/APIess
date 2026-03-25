using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/integracao/clientes")]
public class ClienteController : ControllerBase
{
    private readonly ClienteService _service;

    public ClienteController(ClienteService service)
    {
        _service = service;
    }

    [HttpPost("sync")]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
    {
        var result = await _service.SincronizarClientesAsync(cancellationToken);
        return Ok(result);
    }
}
