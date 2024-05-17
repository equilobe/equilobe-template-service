using Equilobe.TemplateService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Equilobe.TemplateService.Core.Common.Interfaces;

public interface IDbContext
{
    DbSet<UserEntity> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}