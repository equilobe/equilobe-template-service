using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Equilobe.TemplateService.Core.Common.Api;
using Equilobe.TemplateService.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Equilobe.TemplateService.Core.Common.Interfaces
{
    public static class MappingExtensions
    {
        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
            => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();

        public static Task<IPage<TDestination>> ProjectToPageAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration, int pageIndex, int pageSize) where TDestination : class
            => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToPageAsync(pageIndex, pageSize);
    }
}

