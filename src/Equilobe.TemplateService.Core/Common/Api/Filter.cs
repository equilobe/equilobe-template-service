using System;
namespace Equilobe.TemplateService.Core.Common.Api;

public interface ISortFilter
{
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; }
}

public interface IPageFilter
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class PageAndSortFilter : ISortFilter, IPageFilter
{
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

