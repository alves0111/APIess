using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/clientes")]
public class ClienteController : ControllerBase
{
    private readonly ClienteService _service;

    public ClienteController(ClienteService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var clientes = _service.ObterClientes();
        return Ok(clientes);
    }
}