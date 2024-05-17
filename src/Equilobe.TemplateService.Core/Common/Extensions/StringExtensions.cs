using System.Text.RegularExpressions;

namespace Equilobe.TemplateService.Core.Common.Extensions;

public static class StringExtensions
{
    public static string ToCamelCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return char.ToLowerInvariant(value[0]) + value.Substring(1);
    }

    public static string ToValidUrlHyphenCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var resultValue = value;
        resultValue = Regex.Replace(resultValue, @"[^a-zA-Z0-9\s-]", string.Empty);
        resultValue = Regex.Replace(resultValue, @"\s+", " ").Trim();
        resultValue = Regex.Replace(resultValue, @"\s", "-");

        return resultValue;
    }
}
