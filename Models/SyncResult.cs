public sealed class SyncResult
{
    public string Modulo { get; init; } = string.Empty;
    public string Schema { get; init; } = string.Empty;
    public string Tabela { get; init; } = string.Empty;
    public int RegistrosProcessados { get; init; }
    public int LotesProcessados { get; init; }
    public DateTime IniciadoEmUtc { get; init; }
    public DateTime FinalizadoEmUtc { get; init; }
    public double DuracaoSegundos => (FinalizadoEmUtc - IniciadoEmUtc).TotalSeconds;
}
