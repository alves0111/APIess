using System.Globalization;

public static class DbfValueConverter
{
    public static object? ConvertTo(Type targetType, object? value)
    {
        if (value is null || value == DBNull.Value)
            return null;

        var underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (underlying == typeof(string))
        {
            var s = value.ToString()?.Trim();
            return string.IsNullOrWhiteSpace(s) ? null : s;
        }

        if (underlying == typeof(DateOnly))
        {
            if (value is DateTime dt)
                return DateOnly.FromDateTime(dt);

            var s = value.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(s))
                return null;

            if (DateOnly.TryParseExact(s, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOnly))
                return dateOnly;

            if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                return DateOnly.FromDateTime(parsedDate);

            return null;
        }

        if (underlying == typeof(int))
        {
            if (int.TryParse(NormalizeNumber(value), NumberStyles.Any, CultureInfo.InvariantCulture, out var i))
                return i;
            return null;
        }

        if (underlying == typeof(long))
        {
            if (long.TryParse(NormalizeNumber(value), NumberStyles.Any, CultureInfo.InvariantCulture, out var l))
                return l;
            return null;
        }

        if (underlying == typeof(decimal))
        {
            if (decimal.TryParse(NormalizeNumber(value), NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                return d;
            return null;
        }

        if (underlying == typeof(double))
        {
            if (double.TryParse(NormalizeNumber(value), NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                return d;
            return null;
        }

        if (underlying == typeof(bool))
        {
            var s = value.ToString()?.Trim().ToUpperInvariant();
            return s switch
            {
                "T" or "Y" or "S" or "1" => true,
                "F" or "N" or "0" => false,
                _ => null
            };
        }

        return Convert.ChangeType(value, underlying, CultureInfo.InvariantCulture);
    }

    private static string NormalizeNumber(object value)
        => value.ToString()?.Trim().Replace(",", ".") ?? string.Empty;
}
