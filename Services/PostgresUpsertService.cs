using System.Reflection;
using Npgsql;

public class PostgresUpsertService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<PostgresUpsertService> _logger;

    public PostgresUpsertService(
        IConfiguration configuration,
        ILogger<PostgresUpsertService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task UpsertBatchAsync<T>(
        string schema,
        string table,
        IReadOnlyList<string> keyColumns,
        IReadOnlyList<T> items,
        CancellationToken cancellationToken = default)
    {
        if (items.Count == 0)
            return;

        var connectionString = _configuration.GetConnectionString("Postgres");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("ConnectionStrings:Postgres não foi configurada.");

        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead)
            .ToArray();

        var keySet = new HashSet<string>(keyColumns, StringComparer.OrdinalIgnoreCase);
        var updateColumns = properties.Where(p => !keySet.Contains(p.Name)).ToArray();

        var quotedSchema = QuoteIdentifier(schema);
        var quotedTable = QuoteIdentifier(table);

        var sqlColumns = string.Join(", ", properties.Select(p => QuoteIdentifier(p.Name.ToLowerInvariant())));
        var conflictColumns = string.Join(", ", keyColumns.Select(k => QuoteIdentifier(k.ToLowerInvariant())));
        var updateClause = updateColumns.Length == 0
            ? "nothing"
            : "update set " + string.Join(", ", updateColumns.Select(p =>
                $"{QuoteIdentifier(p.Name.ToLowerInvariant())} = excluded.{QuoteIdentifier(p.Name.ToLowerInvariant())}"));

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        var valueLines = new List<string>(items.Count);

        for (var rowIndex = 0; rowIndex < items.Count; rowIndex++)
        {
            var item = items[rowIndex];
            var parameterNames = new List<string>(properties.Length);

            foreach (var property in properties)
            {
                var parameterName = $"p_{rowIndex}_{property.Name.ToLowerInvariant()}";
                parameterNames.Add("@" + parameterName);
                command.Parameters.AddWithValue(parameterName, ToDbValue(property.GetValue(item)));
            }

            valueLines.Add($"({string.Join(", ", parameterNames)})");
        }

        command.CommandText = $@"
insert into {quotedSchema}.{quotedTable} ({sqlColumns})
values {string.Join(",\n     ", valueLines)}
on conflict ({conflictColumns}) do {updateClause};";

        _logger.LogInformation(
            "Executando upsert em {Schema}.{Table} para {Count} registros.",
            schema,
            table,
            items.Count);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static object ToDbValue(object? value)
        => value ?? DBNull.Value;

    private static string QuoteIdentifier(string identifier)
        => $"\"{identifier.Replace("\"", "\"\"")}\"";
}