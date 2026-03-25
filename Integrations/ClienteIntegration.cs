using Microsoft.Extensions.Logging;

public class ClienteIntegration
{
    private readonly DbfReaderService _dbf;
    private readonly ILogger<ClienteIntegration> _logger;

    public ClienteIntegration(
        DbfReaderService dbf,
        ILogger<ClienteIntegration> logger)
    {
        _dbf = dbf;
        _logger = logger;
    }

    public List<ClientDto> BuscarClientes(int skip = 0, int take = 500)
    {
        if (skip < 0)
            throw new ArgumentOutOfRangeException(nameof(skip), "O parâmetro skip não pode ser negativo.");

        if (take <= 0 || take > 5000)
            throw new ArgumentOutOfRangeException(nameof(take), "O parâmetro take deve estar entre 1 e 5000.");

        var clientes = new List<ClientDto>(take);
        var index = 0;

        foreach (var row in _dbf.LerTabela("CLIENTES"))
        {
            if (index++ < skip)
                continue;

            clientes.Add(DbfMapper.MapRow<ClientDto>(row));

            if (clientes.Count >= take)
                break;
        }

        _logger.LogInformation(
            "Lote de clientes lido do DBF. Skip: {Skip}. Take: {Take}. Retornados: {Retornados}",
            skip,
            take,
            clientes.Count);

        return clientes;
    }
}
