using BookStackMcpServer.Models;
using Microsoft.Extensions.Options;

namespace BookStackMcpServer.Middleware;

public class McpSecurityMiddleware
{
    private readonly RequestDelegate _next;
    private readonly SecurityOptions _securityOptions;
    private readonly ILogger<McpSecurityMiddleware> _logger;

    public McpSecurityMiddleware(
        RequestDelegate next,
        IOptions<SecurityOptions> securityOptions,
        ILogger<McpSecurityMiddleware> logger)
    {
        _next = next;
        _securityOptions = securityOptions.Value;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // If security is not configured, allow all requests
        if (string.IsNullOrEmpty(_securityOptions.AuthHeaderName) || 
            string.IsNullOrEmpty(_securityOptions.AuthHeaderValue))
        {
            _logger.LogDebug("Security not configured, allowing request");
            await _next(context);
            return;
        }

        // Check if the request has the required header
        if (!context.Request.Headers.TryGetValue(_securityOptions.AuthHeaderName, out var headerValue))
        {
            _logger.LogWarning("Request missing required authentication header: {HeaderName}", _securityOptions.AuthHeaderName);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Missing authentication header");
            return;
        }

        // Validate the header value
        if (headerValue != _securityOptions.AuthHeaderValue)
        {
            _logger.LogWarning("Invalid authentication header value provided");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid authentication header");
            return;
        }

        _logger.LogDebug("Authentication header validated successfully");
        await _next(context);
    }
}
