using Equilobe.TemplateService.Startup.Extensions;
using Equilobe.TemplateService.Swagger.Extensions;
using Equilobe.TemplateService.Infrastructure.ExceptionHandling.Extensions;
using Equilobe.TemplateService.Infrastructure.Services.Extensions;
using Equilobe.TemplateService.Infrastructure.Mediator.Extensions;
using Equilobe.TemplateService.Infrastructure.Database.Extensions;
using Equilobe.TemplateService.Infrastructure.FluentValidation.Extensions;
using Equilobe.TemplateService.Authorization.Extensions;
using Equilobe.TemplateService.Authentication.Extensions;
using Equilobe.TemplateService.Core.Features.Users.CreateUser;
using Equilobe.TemplateService.Cors;
using System.Reflection;

var (builder, services, configuration) = WebApplication.CreateBuilder(args);
var executingAssembly = Assembly.GetExecutingAssembly();
var coreAssembly = typeof(CreateUserCommand).Assembly;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwagger();
services.AddMediator(executingAssembly, coreAssembly);
services.AddAutoMapper(executingAssembly, coreAssembly);
services.AddCors(configuration);
services.AddOwnAuthentication(configuration);
services.AddOwnAuthorization();
services.AddExceptionHandlers();
services.AddExceptionHandlersFrom(executingAssembly); 
services.AddPostgreSql(configuration);
services.AddInfrastructureServices();
services.AddValidators(coreAssembly);
services.AddHttpContextAccessor();

var (middleware, endpoints, app)
    = builder.Build();

middleware.UseCors(PolicyNames.AllowOrigin);
middleware.UseSwagger();
middleware.UseSwaggerUI();
middleware.UseHttpsRedirection();
middleware.UseRouting();
middleware.UseAuthentication();
middleware.UseAuthorization();
middleware.UseExceptionHandling();
middleware.UseExceptionLogging();

endpoints.MapControllers()
    .RequireAuthorization(Equilobe.TemplateService.Core.Common.Auth.PolicyNames.DefaultPolicy);

if (app.Environment.IsEnvironment("Local") || 
    app.Environment.IsDevelopment() || 
    app.Environment.IsEnvironment("IntegrationTest"))
{
    app.ApplyMigrations();
}

app.Run();

public partial class Program { }
