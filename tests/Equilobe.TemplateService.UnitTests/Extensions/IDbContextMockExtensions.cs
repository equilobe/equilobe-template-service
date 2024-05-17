using Equilobe.TemplateService.Core.Common.Interfaces;
using NSubstitute;

namespace Equilobe.TemplateService.UnitTests.Extensions;

internal static class IDbContextMock
{
    public static async Task ReceivedSavedChangesAsync(this IDbContext dbContextMock, int times = 1)
    {
        await dbContextMock
            .Received(times)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
