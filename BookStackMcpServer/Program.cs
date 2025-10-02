using BookStackMcpServer.Models;
using BookStackMcpServer.Services;
using BookStackMcpServer.HealthChecks;
using BookStackApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Services.Configure<BookStackOptions>(
    builder.Configuration.GetSection(BookStackOptions.SectionName));

// Add BookStack API client
builder.Services.AddSingleton<ApiService>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var bookStackOptions = configuration.GetSection(BookStackOptions.SectionName).Get<BookStackOptions>()
        ?? throw new InvalidOperationException("BookStack configuration is required");
    
    return new ApiService(bookStackOptions.BaseUrl, bookStackOptions.TokenId, bookStackOptions.TokenSecret);
});

// Add MCP tools
builder.Services.AddSingleton<BookStackMcpTools>();

// Add health checks
builder.Services.AddHealthChecks()
    .AddCheck<BookStackHealthCheck>("bookstack", tags: new[] { "ready" });

var app = builder.Build();

// MCP Server endpoint using Server-Sent Events transport
app.MapGet("/mcp", async (HttpContext context, BookStackMcpTools tools, IServiceProvider serviceProvider) =>
{
    context.Response.Headers.Add("Content-Type", "text/event-stream");
    context.Response.Headers.Add("Cache-Control", "no-cache");
    context.Response.Headers.Add("Connection", "keep-alive");
    
    // Basic MCP server info
    var serverInfo = new
    {
        name = "bookstack-server",
        version = "1.0.0",
        tools = GetToolsInfo()
    };
    
    await context.Response.WriteAsync($"data: {System.Text.Json.JsonSerializer.Serialize(serverInfo)}\n\n");
    await context.Response.Body.FlushAsync();
    
    // Keep connection alive
    while (!context.RequestAborted.IsCancellationRequested)
    {
        await Task.Delay(1000, context.RequestAborted);
        await context.Response.WriteAsync("data: {\"type\":\"ping\"}\n\n");
        await context.Response.Body.FlushAsync();
    }
});

// HTTP endpoints for development and testing
app.MapGet("/tools", () => Results.Ok(new { tools = GetToolsInfo() }));

app.MapGet("/test", async (BookStackMcpTools tools) =>
{
    try
    {
        var books = await tools.ListBooksAsync();
        return Results.Ok(new { status = "success", message = "BookStack API connected", data = books });
    }
    catch (Exception ex)
    {
        return Results.Problem($"BookStack API error: {ex.Message}");
    }
});

app.MapPost("/invoke/{toolName}", async (BookStackMcpTools tools, string toolName, Dictionary<string, object>? parameters) =>
{
    try
    {
        var method = typeof(BookStackMcpTools).GetMethods()
            .Where(m => m.GetCustomAttributes(typeof(McpServerToolAttribute), false).Any())
            .FirstOrDefault(m => 
            {
                var attr = (McpServerToolAttribute?)m.GetCustomAttributes(typeof(McpServerToolAttribute), false).FirstOrDefault();
                return attr?.Name == toolName;
            });

        if (method == null)
        {
            return Results.NotFound($"Tool '{toolName}' not found");
        }

        var methodParams = method.GetParameters();
        var args = new object[methodParams.Length];
        
        for (int i = 0; i < methodParams.Length; i++)
        {
            var param = methodParams[i];
            if (parameters?.ContainsKey(param.Name!) == true)
            {
                var value = parameters[param.Name!];
                
                // Handle nullable types
                if (param.ParameterType.IsGenericType && param.ParameterType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var underlyingType = Nullable.GetUnderlyingType(param.ParameterType);
                    args[i] = value == null ? null : Convert.ChangeType(value, underlyingType!);
                }
                else
                {
                    args[i] = Convert.ChangeType(value, param.ParameterType);
                }
            }
            else if (param.HasDefaultValue)
            {
                args[i] = param.DefaultValue!;
            }
            else
            {
                return Results.BadRequest($"Missing required parameter: {param.Name}");
            }
        }

        var result = await (Task<string>)method.Invoke(tools, args)!;
        return Results.Ok(new { result });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error invoking tool: {ex.Message}");
    }
});

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

static object[] GetToolsInfo()
{
    return typeof(BookStackMcpTools).GetMethods()
        .Where(m => m.GetCustomAttributes(typeof(McpServerToolAttribute), false).Any())
        .Select(m => 
        {
            var toolAttr = (McpServerToolAttribute?)m.GetCustomAttributes(typeof(McpServerToolAttribute), false).FirstOrDefault();
            var descAttr = (System.ComponentModel.DescriptionAttribute?)m.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false).FirstOrDefault();
            
            return new
            {
                name = toolAttr?.Name,
                description = descAttr?.Description,
                parameters = m.GetParameters().Select(p => new { 
                    name = p.Name, 
                    type = GetFriendlyTypeName(p.ParameterType),
                    required = !p.HasDefaultValue,
                    defaultValue = p.HasDefaultValue ? p.DefaultValue?.ToString() : null
                }).ToArray()
            };
        })
        .ToArray();
}

static string GetFriendlyTypeName(Type type)
{
    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
    {
        return $"{Nullable.GetUnderlyingType(type)!.Name}?";
    }
    return type.Name;
}
