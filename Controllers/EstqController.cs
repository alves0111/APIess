using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/estoque")]
public class EstqController : ControllerBase
{
    private readonly EstqService _service;

    public EstqController(EstqService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] int skip = 0, [FromQuery] int take = 200)
    {
        var estq = _service.ObterEstq(skip, take);

        return Ok(new
        {
            skip,
            take,
            count = estq.Count,
            items = estq
        });
    }
}