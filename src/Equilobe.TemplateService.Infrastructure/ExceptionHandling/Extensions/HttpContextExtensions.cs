using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Equilobe.TemplateService.Infrastructure.ExceptionHandling.Extensions;

public static class HttpContextExtensions
{
    public static async Task SetProblemDetailsResponse(this HttpContext context, ProblemDetails details)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = details.Status ?? StatusCodes.Status500InternalServerError;
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        string serializedBody = JsonConvert.SerializeObject(details, settings);

        await context.Response.WriteAsync(serializedBody);
    }
}