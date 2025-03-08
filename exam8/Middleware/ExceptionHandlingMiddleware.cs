using System.Net;
using System.Text.Json;

namespace exam8.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleException(context, e);
        }
    }

    private static Task HandleException(HttpContext context, Exception exception)
    {
        var response = new
        {
            message = exception.Message,
            statusCode = (int)HttpStatusCode.InternalServerError
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return  context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}