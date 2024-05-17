using System.Reflection;
using MediatR;

namespace Equilobe.TemplateService.Core.Common.Extensions;

public static class RequestQueryExtensions
{
    public static string ToQueryString<T>(this IRequest<T> query)
    {
        var properties = query.GetType().GetProperties();

        var queryString = string.Join("&", properties
            .Where(p => p.GetValue(query) is not null)
            .Select(p => WritePropertyValue(query, p)));

        return queryString;
    }

    private static string WritePropertyValue<T>(T query, PropertyInfo propertyInfo)
    {
        if (propertyInfo.PropertyType.IsArray)
        {
            return WriteArrayValues(query, propertyInfo);
        }

        var value = propertyInfo.GetValue(query);
        return $"{propertyInfo.Name}={value}";
    }

    private static string WriteArrayValues<T>(T query, PropertyInfo propertyInfo)
    {
        var elementType = propertyInfo.PropertyType.GetElementType();
        if (elementType is null)
        {
            return string.Empty;
        }

        var array = (Array?)propertyInfo.GetValue(query);
        if (array is null)
        {
            return string.Empty;
        }

        int arrayLength = array.GetLength(0);
        if (arrayLength == 0)
        {
            return string.Empty;
        }

        return string.Join("&", array.Cast<object>().Select(x => $"{propertyInfo.Name}={x}"));
    }
}
