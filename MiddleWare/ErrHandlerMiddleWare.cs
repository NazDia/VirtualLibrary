
using System.Text.Json;

namespace VirtualLibrary.MiddleWare;

public class ErrHandlerMiddleWare(RequestDelegate next, IConfiguration configuration) : IMiddleware
{
    private readonly RequestDelegate _next = next;
    private readonly IConfiguration _config = configuration;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try {
            await _next(context);
        }
        catch (Exception ex) {
            context.Response.StatusCode = 500;
            var result = JsonSerializer.Serialize(new { message = _config["ErrorMsg"], ex.Message });
            await context.Response.WriteAsJsonAsync(result);
        }
    }
}