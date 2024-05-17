using System.Threading.Tasks;

namespace Equilobe.TemplateService.Core.Common.Interfaces;

public interface IUserProvider
{
    Task<long> GetCurrentUserIdAsync();
    Task<long?> GetCurrentUserIdOrDefaultAsync();
}