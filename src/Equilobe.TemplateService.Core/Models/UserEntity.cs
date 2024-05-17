using System;
namespace Equilobe.TemplateService.Core.Models
{
    public class UserEntity : IRootEntity
    {
        public long Id { get; set; } = default!;
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? LastUpdatedAtUtc { get; set; }
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string ExternalId { get; set; } = default!;
        public string? ProfilePictureUrl { get; set; } 
    }
}