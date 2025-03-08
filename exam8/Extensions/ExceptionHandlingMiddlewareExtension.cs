using exam8.Middleware;
using Microsoft.AspNetCore.Diagnostics;

namespace exam8.Extensions;

public static class ExceptionHandlingMiddlewareExtension
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}