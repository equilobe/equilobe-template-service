using System;
using Equilobe.TemplateService.Core.Common.Api;

namespace Equilobe.TemplateService.Core.Common.Extensions;

public static class IEnumerableExtensions
{
    public static Page<T> ToPage<T>(this IEnumerable<T> collection, int totalCount)
    {
        return new Page<T>(
            totalCount: totalCount,
            items: collection.ToList());
    }
}