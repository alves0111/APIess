namespace MEUERP.API2.Config;

public sealed class IntegracaoConfig
{
    public string DbfPath { get; set; } = string.Empty;
    public string TempPath { get; set; } = string.Empty;
    public int SyncBatchSize { get; set; } = 500;
    public ModulosConfig Modulos { get; set; } = new();
}

public sealed class ModulosConfig
{
    public bool Clientes { get; set; } = true;
    public bool Produtos { get; set; }
    public bool Estoques { get; set; }
    public bool Pedidos { get; set; }
}