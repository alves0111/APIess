using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/integracao/produtos")]
public class ProdController : ControllerBase
{
    private readonly ProdService _service;

    public ProdController(ProdService service)
    {
        _service = service;
    }

    [HttpPost("sync")]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
    {
        var result = await _service.SincronizarProdutosAsync(cancellationToken);
        return Ok(result);
    }
}
