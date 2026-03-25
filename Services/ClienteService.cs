using MEUERP.API2.Config;
using Microsoft.Extensions.Options;

public class ClienteService
{
    private readonly ClienteIntegration _integration;
    private readonly IntegracaoConfig _config;

    public ClienteService(
        ClienteIntegration integration,
        IOptions<IntegracaoConfig> config)
    {
        _integration = integration;
        _config = config.Value;
    }

    public IReadOnlyList<ClientDto> ObterClientes(int skip = 0, int take = 200)
    {
        if (!_config.Modulos.Clientes)
        {
            throw new InvalidOperationException("O módulo de clientes está desabilitado na configuração.");
        }

        return _integration.BuscarClientes(skip, take);
    }
}