using MEUERP.API2.Config;
using Microsoft.Extensions.Options;

public class ProdService
{
    private readonly ProdIntegration _integration;
    private readonly IntegracaoConfig _config;

    public ProdService(
        ProdIntegration integration,
        IOptions<IntegracaoConfig> config)
    {
        _integration = integration;
        _config = config.Value;
    }

    public IReadOnlyList<ProdDto> ObterProdutos(int skip = 0, int take = 200)
    {
        if (!_config.Modulos.Produtos)
        {
            throw new InvalidOperationException("Módulo de produtos desabilitado.");
        }

        return _integration.BuscarProdutos(skip, take);
    }
}