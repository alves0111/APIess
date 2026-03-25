using MEUERP.API2.Config;
using Microsoft.Extensions.Options;

public class ClienteService
{
    private readonly ClienteIntegration _integration;
    private readonly PostgresUpsertService _upsert;
    private readonly IntegracaoConfig _config;
    private readonly ILogger<ClienteService> _logger;

    public ClienteService(
        ClienteIntegration integration,
        PostgresUpsertService upsert,
        IOptions<IntegracaoConfig> config,
        ILogger<ClienteService> logger)
    {
        _integration = integration;
        _upsert = upsert;
        _config = config.Value;
        _logger = logger;
    }

    public async Task<SyncResult> SincronizarClientesAsync(CancellationToken cancellationToken = default)
    {
        if (!_config.Modulos.Clientes)
            throw new InvalidOperationException("O módulo de clientes está desabilitado na configuração.");

        var startedAt = DateTime.UtcNow;
        var skip = 0;
        var take = _config.SyncBatchSize;
        var total = 0;
        var batches = 0;

        while (true)
        {
            var lote = _integration.BuscarClientes(skip, take);
            if (lote.Count == 0)
                break;

            await _upsert.UpsertBatchAsync("meuerp", "clientes", new[] { "COD_CLI" }, lote, cancellationToken);

            total += lote.Count;
            batches++;
            skip += lote.Count;

            _logger.LogInformation(
                "Lote de clientes sincronizado. Lote: {Lote}. Registros no lote: {RegistrosLote}. Total: {Total}",
                batches,
                lote.Count,
                total);

            if (lote.Count < take)
                break;
        }

        return new SyncResult
        {
            Modulo = "clientes",
            Schema = "meuerp",
            Tabela = "clientes",
            RegistrosProcessados = total,
            LotesProcessados = batches,
            IniciadoEmUtc = startedAt,
            FinalizadoEmUtc = DateTime.UtcNow
        };
    }
}
