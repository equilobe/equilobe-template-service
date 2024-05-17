
using System;
using Equilobe.TemplateService.Core.Common.Interfaces;
namespace Equilobe.TemplateService.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetUtcNow() => DateTime.UtcNow;
    }
}

