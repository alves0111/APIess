using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/produtos")]
public class ProdController : ControllerBase
{
    private readonly ProdService _service;

    public ProdController(ProdService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] int skip = 0, [FromQuery] int take = 200)
    {
        var produtos = _service.ObterProdutos(skip, take);

        return Ok(new
        {
            skip,
            take,
            count = produtos.Count,
            items = produtos
        });
    }
}