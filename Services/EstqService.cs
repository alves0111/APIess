using MEUERP.API2.Config;
using Microsoft.Extensions.Options;

public class EstqService
{
    private readonly EstqIntegration _integration;
    private readonly PostgresUpsertService _upsert;
    private readonly IntegracaoConfig _config;
    private readonly ILogger<EstqService> _logger;

    public EstqService(
        EstqIntegration integration,
        PostgresUpsertService upsert,
        IOptions<IntegracaoConfig> config,
        ILogger<EstqService> logger)
    {
        _integration = integration;
        _upsert = upsert;
        _config = config.Value;
        _logger = logger;
    }

    public async Task<SyncResult> SincronizarEstoquesAsync(CancellationToken cancellationToken = default)
    {
        if (!_config.Modulos.Estoques)
            throw new InvalidOperationException("O módulo de estoques está desabilitado na configuração.");

        var startedAt = DateTime.UtcNow;
        var skip = 0;
        var take = _config.SyncBatchSize;
        var total = 0;
        var batches = 0;

        while (true)
        {
            var lote = _integration.BuscarEstq(skip, take);
            if (lote.Count == 0)
                break;

            await _upsert.UpsertBatchAsync("meuerp", "estoques", new[] { "COD_EST", "COD_PRO" }, lote, cancellationToken);

            total += lote.Count;
            batches++;
            skip += lote.Count;

            _logger.LogInformation(
                "Lote de estoques sincronizado. Lote: {Lote}. Registros no lote: {RegistrosLote}. Total: {Total}",
                batches,
                lote.Count,
                total);

            if (lote.Count < take)
                break;
        }

        return new SyncResult
        {
            Modulo = "estoques",
            Schema = "meuerp",
            Tabela = "estoques",
            RegistrosProcessados = total,
            LotesProcessados = batches,
            IniciadoEmUtc = startedAt,
            FinalizadoEmUtc = DateTime.UtcNow
        };
    }
}
