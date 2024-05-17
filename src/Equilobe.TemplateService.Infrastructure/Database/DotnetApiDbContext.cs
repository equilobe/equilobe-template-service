using System;
using Equilobe.TemplateService.Core.Common.Interfaces;
using Equilobe.TemplateService.Core.Models;
using Equilobe.TemplateService.Infrastructure.Database.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Equilobe.TemplateService.Infrastructure.Database
{
    public class AppDbContext : DbContext, IDbContext
    {
        public virtual DbSet<UserEntity> Users { get; set; } = default!;

        private AuditableEntitySaveChangesInterceptor? auditableEntitySaveChangesInterceptor;
  
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (auditableEntitySaveChangesInterceptor == null)
                return;

            optionsBuilder.AddInterceptors(auditableEntitySaveChangesInterceptor);
        }

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            AuditableEntitySaveChangesInterceptor? auditableEntitySaveChangesInterceptor) : base(options)
        {
            this.auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

        public AppDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}

