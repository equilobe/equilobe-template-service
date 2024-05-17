using System;
namespace Equilobe.TemplateService.Core.Models
{
    public interface IEntity { }

    public interface IRootEntity : IEntity
    {
        long Id { get; set; }
    }

    public interface IAuditableEntity : IRootEntity
    {
        DateTime CreatedAtUtc { get; set; }
        DateTime? LastUpdatedAtUtc { get; set; }
    }
}

