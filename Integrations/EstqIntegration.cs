using Microsoft.Extensions.Logging;

public class EstqIntegration
{
    private readonly DbfReaderService _dbf;
    private readonly ILogger<EstqIntegration> _logger;

    public EstqIntegration(
        DbfReaderService dbf,
        ILogger<EstqIntegration> logger)
    {
        _dbf = dbf;
        _logger = logger;
    }

    public List<EstqDto> BuscarEstq(int skip = 0, int take = 500)
    {
        if (skip < 0)
            throw new ArgumentOutOfRangeException(nameof(skip), "O parâmetro skip não pode ser negativo.");

        if (take <= 0 || take > 5000)
            throw new ArgumentOutOfRangeException(nameof(take), "O parâmetro take deve estar entre 1 e 5000.");

        var estoques = new List<EstqDto>(take);
        var index = 0;

        foreach (var row in _dbf.LerTabela("ESTOQUES"))
        {
            if (index++ < skip)
                continue;

            estoques.Add(DbfMapper.MapRow<EstqDto>(row));

            if (estoques.Count >= take)
                break;
        }

        _logger.LogInformation(
            "Lote de estoques lido do DBF. Skip: {Skip}. Take: {Take}. Retornados: {Retornados}",
            skip,
            take,
            estoques.Count);

        return estoques;
    }
}
