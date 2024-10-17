using System.Net;

namespace luna2000.Middlewares;

public class IpRestrictionMiddleware
{
    private readonly RequestDelegate _next;

    public IpRestrictionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/api/"))
        {
            await _next(context);
            return;
        }

        var allowedIps = new[] { "127.0.0.1", "::1" };

        var remoteIp = context.Connection.RemoteIpAddress;

        Console.WriteLine(remoteIp);

        if (!allowedIps.Contains(remoteIp?.ToString()))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await context.Response.WriteAsync("Forbidden");
            return;
        }

        await _next(context);
    }
}