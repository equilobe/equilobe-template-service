using Swashbuckle.AspNetCore.Annotations;

namespace Equilobe.TemplateService.Core.Features.Users;

public class UserSwaggerOperationAttribute : SwaggerOperationAttribute
{
    private const string Tag = "Users";
    public UserSwaggerOperationAttribute() : base()
    {
        Tags = [Tag];
    }
}
