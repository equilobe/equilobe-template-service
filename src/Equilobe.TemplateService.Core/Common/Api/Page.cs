using System;
namespace Equilobe.TemplateService.Core.Common.Api;

public interface IPage<T>
{
    public int TotalCount { get; }
    public IEnumerable<T> Items { get; }
}

public class Page<T> : IPage<T>
{
    public int TotalCount { get; }
    public IEnumerable<T> Items { get; }

    public Page(int totalCount, IEnumerable<T> items)
    {
        TotalCount = totalCount;
        Items = items;
    }
}

