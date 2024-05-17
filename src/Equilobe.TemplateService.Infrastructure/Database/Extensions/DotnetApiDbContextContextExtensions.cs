using System;
using Equilobe.TemplateService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Equilobe.TemplateService.Infrastructure.Database.Extensions
{
    public static class AppDbContextContextExtensions
    {
        public static async Task<UserEntity?> GetUserOrDefaultAsync(this AppDbContext dbContext, long userId) => await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
}

