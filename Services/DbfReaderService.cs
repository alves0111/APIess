using System.Text;
using MEUERP.API2.Config;
using Microsoft.Extensions.Options;
using NDbfReader;

public class DbfReaderService
{
    private readonly IntegracaoConfig _config;
    private readonly ILogger<DbfReaderService> _logger;

    public DbfReaderService(
        IOptions<IntegracaoConfig> config,
        ILogger<DbfReaderService> logger)
    {
        _config = config.Value;
        _logger = logger;
    }

    public IEnumerable<Dictionary<string, object?>> LerTabela(string nomeTabela)
    {
        if (string.IsNullOrWhiteSpace(nomeTabela))
        {
            throw new ArgumentException("O nome da tabela é obrigatório.", nameof(nomeTabela));
        }

        var origemDbf = Path.Combine(_config.DbfPath, $"{nomeTabela}.DBF");
        var origemFpt = Path.Combine(_config.DbfPath, $"{nomeTabela}.FPT");

        if (!File.Exists(origemDbf))
        {
            throw new FileNotFoundException($"Arquivo DBF não encontrado: {origemDbf}", origemDbf);
        }

        Directory.CreateDirectory(_config.TempPath);

        var destinoDbf = Path.Combine(_config.TempPath, $"{nomeTabela}_COPY.DBF");
        var destinoFpt = Path.Combine(_config.TempPath, $"{nomeTabela}_COPY.FPT");

        CopiarArquivoSeguro(origemDbf, destinoDbf);

        if (File.Exists(origemFpt))
        {
            CopiarArquivoSeguro(origemFpt, destinoFpt);
        }
        else
        {
            _logger.LogDebug(
                "Arquivo FPT não encontrado para a tabela {Tabela}. Caminho: {Caminho}",
                nomeTabela,
                origemFpt);
        }

        _logger.LogInformation(
            "Tabela {Tabela} copiada para leitura temporária em {Destino}",
            nomeTabela,
            destinoDbf);

        using var table = Table.Open(destinoDbf);
        var reader = table.OpenReader(Encoding.GetEncoding("ISO-8859-1"));

        while (reader.Read())
        {
            var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

            foreach (var column in table.Columns)
            {
                row[column.Name] = reader.GetValue(column.Name);
            }

            yield return row;
        }
    }

    private static void CopiarArquivoSeguro(string origem, string destino)
    {
        using var source = new FileStream(
            origem,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite);

        using var dest = new FileStream(
            destino,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None);

        source.CopyTo(dest);
    }
}