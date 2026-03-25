using Microsoft.Extensions.Logging;

public class ProdIntegration
{
    private readonly DbfReaderService _dbf;
    private readonly ILogger<ProdIntegration> _logger;

    public ProdIntegration(
        DbfReaderService dbf,
        ILogger<ProdIntegration> logger)
    {
        _dbf = dbf;
        _logger = logger;
    }

    public List<ProdDto> BuscarProdutos(int skip = 0, int take = 500)
    {
        if (skip < 0)
            throw new ArgumentOutOfRangeException(nameof(skip), "O parâmetro skip não pode ser negativo.");

        if (take <= 0 || take > 5000)
            throw new ArgumentOutOfRangeException(nameof(take), "O parâmetro take deve estar entre 1 e 5000.");

        var produtos = new List<ProdDto>(take);
        var index = 0;

        foreach (var row in _dbf.LerTabela("PRODUTOS"))
        {
            if (index++ < skip)
                continue;

            produtos.Add(DbfMapper.MapRow<ProdDto>(row));

            if (produtos.Count >= take)
                break;
        }

        _logger.LogInformation(
            "Lote de produtos lido do DBF. Skip: {Skip}. Take: {Take}. Retornados: {Retornados}",
            skip,
            take,
            produtos.Count);

        return produtos;
    }
}
