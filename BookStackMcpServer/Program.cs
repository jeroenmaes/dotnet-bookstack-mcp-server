using BookStackMcpServer.Models;
using BookStackMcpServer.Services;
using BookStackApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;

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

// Add MCP Server with HTTP transport and BookStack tools
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithTools<BookStackMcpTools>();

var app = builder.Build();

// Map MCP endpoint using the SDK
app.MapMcp();

app.Run();
