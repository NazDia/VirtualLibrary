
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace VirtualLibrary.MiddleWare;

public class ErrHandlerMiddleWareOptions {
    public string ErrorMsg { get; set; } = "";
}

public class ErrHandlerMiddleWare(RequestDelegate next, IOptions<ErrHandlerMiddleWareOptions> options)
{
    private readonly RequestDelegate _next = next;
    private readonly ErrHandlerMiddleWareOptions _options = options.Value;

    public async Task InvokeAsync(HttpContext context)
    {
        // Tries to handle the request normally.
        try {
            await _next(context);
        }
        // If it fails it sends a 500 code error msg along with a json depicting the error.
        catch (Exception ex) {
            context.Response.StatusCode = 500;
            var result = JsonSerializer.Serialize(new { message = _options.ErrorMsg, description = ex.Message });
            await context.Response.WriteAsJsonAsync(result);
        }
    }
}

public static class ErrHandlerMiddleWareExtension {
    public static IApplicationBuilder UseErrHandlerMiddleware(
        this IApplicationBuilder builder
    ) {
        return builder.UseMiddleware<ErrHandlerMiddleWare>();
    }
}