using BookStackMcpServer.Models;
using BookStackMcpServer.Services;
using BookStackMcpServer.HealthChecks;
using BookStackMcpServer.Middleware;
using BookStackApiClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using System.Text.Json;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Services.Configure<BookStackOptions>(
    builder.Configuration.GetSection(BookStackOptions.SectionName));
builder.Services.Configure<SecurityOptions>(
    builder.Configuration.GetSection(SecurityOptions.SectionName));
builder.Services.Configure<ThrottlingOptions>(
    builder.Configuration.GetSection(ThrottlingOptions.SectionName));

// Add BookStack API client
builder.Services.AddSingleton<BookStackClient>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var bookStackOptions = configuration.GetSection(BookStackOptions.SectionName).Get<BookStackOptions>()
        ?? throw new InvalidOperationException("BookStack configuration is required");
    
    // The new API requires a Uri that ends with /api/
    var baseUrl = bookStackOptions.BaseUrl.TrimEnd('/');
    var apiUri = new Uri($"{baseUrl}/api/");
    
    return new BookStackClient(apiUri, bookStackOptions.TokenId, bookStackOptions.TokenSecret);
});

// Add MCP Server with HTTP transport and BookStack tools
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithTools<BookStackMcpTools>();

// Add health checks
builder.Services.AddHealthChecks()
    .AddCheck<BookStackHealthCheck>("bookstack", tags: new[] { "ready" });

// Configure rate limiting
var throttlingOptions = builder.Configuration.GetSection(ThrottlingOptions.SectionName).Get<ThrottlingOptions>()
    ?? new ThrottlingOptions();

if (throttlingOptions.Enabled)
{
    builder.Services.AddRateLimiter(options =>
    {
        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                factory: partition => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = throttlingOptions.PermitLimit,
                    Window = TimeSpan.FromSeconds(throttlingOptions.WindowSeconds),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = throttlingOptions.QueueLimit
                }));

        options.OnRejected = async (context, cancellationToken) =>
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken);
        };
    });
}

var app = builder.Build();

// Apply rate limiting to MCP endpoints (exclude health checks)
if (throttlingOptions.Enabled)
{
    app.UseWhen(
        context => !context.Request.Path.StartsWithSegments("/health"),
        appBuilder => appBuilder.UseRateLimiter()
    );
}

// Apply security middleware to MCP endpoints
app.UseWhen(
    context => !context.Request.Path.StartsWithSegments("/health"),
    appBuilder => appBuilder.UseMiddleware<McpSecurityMiddleware>()
);

// Map MCP endpoint using the SDK
app.MapMcp();

// Health check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                data = e.Value.Data
            }),
            totalDuration = report.TotalDuration
        });
        await context.Response.WriteAsync(result);
    }
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        });
        await context.Response.WriteAsync(result);
    }
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false, // No checks, just returns 200 if app is running
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = "Healthy"
        });
        await context.Response.WriteAsync(result);
    }
});

app.Run();
