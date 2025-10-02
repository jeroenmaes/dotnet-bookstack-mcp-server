using BookStackMcpServer.Models;
using BookStackMcpServer.Services;
using BookStackMcpServer.HealthChecks;
using BookStackApiClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Services.Configure<BookStackOptions>(
    builder.Configuration.GetSection(BookStackOptions.SectionName));

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

var app = builder.Build();

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
