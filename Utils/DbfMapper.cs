using System.Reflection;
using Microsoft.Extensions.Logging;

public static class DbfMapper
{
    public static T MapRow<T>(IReadOnlyDictionary<string, object?> row) where T : new()
    {
        var entity = new T();

        foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!prop.CanWrite)
                continue;

            if (!row.TryGetValue(prop.Name, out var rawValue))
                continue;

            var converted = DbfValueConverter.ConvertTo(prop.PropertyType, rawValue);
            prop.SetValue(entity, converted);
        }

        return entity;
    }
}
