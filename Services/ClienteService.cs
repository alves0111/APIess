public class ClienteService
{
    private readonly ClienteIntegration _integration;

    public ClienteService(ClienteIntegration integration)
    {
        _integration = integration;
    }

    public List<ClientDto> ObterClientes()
    {
        var clientes = _integration.BuscarClientes();

        // Aqui você pode tratar regras
        return clientes;
    }
}