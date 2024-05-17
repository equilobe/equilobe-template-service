using Equilobe.TemplateService.Core.Models;
using Equilobe.TemplateService.Infrastructure.Database;

namespace Equilobe.TemplateService.IntegrationTests.Extensions;

public static class AppDbContextExtensions
{
    public static void ReloadAllEntities(this AppDbContext dbContext)
    {
        dbContext.ChangeTracker.Entries().ToList().ForEach(e => e.Reload());
    }

    public static void ReloadEntity(this AppDbContext dbContext, IEntity entity)
    {
        dbContext.Entry(entity).Reload();
    }

    public static T InsertEntityAndSave<T>(this AppDbContext context, T entity) where T : class
    {
        context.Add(entity);
        context.SaveChanges();
        return entity;
    }

    public static async Task<T> InsertEntityAndSaveAsync<T>(this AppDbContext context, T entity) where T : class
    {
        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public static void InsertEntitiesAndSave<T>(this AppDbContext context, IEnumerable<T> entities) where T : class
    {
        context.AddRange(entities);
        context.SaveChanges();
    }

    public static async Task InsertEntitiesAndSaveAsync<T>(this AppDbContext context, IEnumerable<T> entities) where T : class
    {
        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public static async Task CleanUpDatabaseAsync(this AppDbContext context)
    {
        context.RemoveDbEntities();
        await context.SaveChangesAsync();
    }

    public static void CleanUpDatabase(this AppDbContext context)
    {
        context.RemoveDbEntities();

        context.SaveChanges();
    }

    private static void RemoveDbEntities(this AppDbContext context)
    {
        context.RemoveRange(context.Users);
    }
}
