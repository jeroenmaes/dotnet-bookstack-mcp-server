using BookStackMcpServer.Models;
using BookStackMcpServer.Services;
using BookStackApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using ModelContextProtocol.Server;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Services.Configure<BookStackOptions>(
    builder.Configuration.GetSection(BookStackOptions.SectionName));

// Add BookStack API client
builder.Services.AddSingleton<ApiService>(serviceProvider =>
{
    var bookStackOptions = builder.Configuration.GetSection(BookStackOptions.SectionName).Get<BookStackOptions>()
        ?? throw new InvalidOperationException("BookStack configuration is required");
    
    return new ApiService(bookStackOptions.BaseUrl, bookStackOptions.TokenId, bookStackOptions.TokenSecret);
});

// Add MCP services
builder.Services.AddSingleton<BookStackMcpTools>();

// Add MCP server
builder.Services.AddMcpServer()
    .WithServerInfo("bookstack-server", "1.0.0")
    .WithToolsFromService<BookStackMcpTools>();

var app = builder.Build();

// Configure MCP server routing
app.MapMcpEndpoints();

app.Run();
