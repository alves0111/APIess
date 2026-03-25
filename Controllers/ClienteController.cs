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
    public IActionResult Get([FromQuery] int skip = 0, [FromQuery] int take = 200)
    {
        var clientes = _service.ObterClientes(skip, take);

        return Ok(new
        {
            skip,
            take,
            count = clientes.Count,
            items = clientes
        });
    }
}