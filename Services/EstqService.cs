using MEUERP.API2.Config;
using Microsoft.Extensions.Options;

public class EstqService
{
    private readonly EstqIntegration _integration;
    private readonly IntegracaoConfig _config;

    public EstqService(
        EstqIntegration integration,
        IOptions<IntegracaoConfig> config)
    {
        _integration = integration;
        _config = config.Value;
    }

    public IReadOnlyList<EstqDto> ObterEstq(int skip = 0, int take = 200)
    {
        if (!_config.Modulos.Estoques)
        {
            throw new InvalidOperationException("Módulo de estoque desabilitado.");
        }

        return _integration.BuscarEstq(skip, take);
    }
}