using BookStackMcpServer.Middleware;
using BookStackMcpServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BookStackMcpServer.Tests.Middleware;

public class McpSecurityMiddlewareTests
{
    private readonly Mock<ILogger<McpSecurityMiddleware>> _loggerMock;
    
    public McpSecurityMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<McpSecurityMiddleware>>();
    }

    [Fact]
    public async Task InvokeAsync_WhenSecurityNotConfigured_AllowsRequest()
    {
        // Arrange
        var securityOptions = new SecurityOptions
        {
            AuthHeaderName = null,
            AuthHeaderValue = null
        };
        var optionsMock = Options.Create(securityOptions);
        var context = new DefaultHttpContext();
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        var middleware = new McpSecurityMiddleware(next, optionsMock, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(nextCalled);
        Assert.NotEqual(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WhenAuthHeaderNameNotConfigured_AllowsRequest()
    {
        // Arrange
        var securityOptions = new SecurityOptions
        {
            AuthHeaderName = null,
            AuthHeaderValue = "some-value"
        };
        var optionsMock = Options.Create(securityOptions);
        var context = new DefaultHttpContext();
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        var middleware = new McpSecurityMiddleware(next, optionsMock, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(nextCalled);
    }

    [Fact]
    public async Task InvokeAsync_WhenAuthHeaderMissing_Returns401()
    {
        // Arrange
        var securityOptions = new SecurityOptions
        {
            AuthHeaderName = "X-Auth-Token",
            AuthHeaderValue = "secret-token"
        };
        var optionsMock = Options.Create(securityOptions);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        var middleware = new McpSecurityMiddleware(next, optionsMock, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(context.Response.Body).ReadToEnd();
        Assert.Contains("Missing authentication header", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_WhenAuthHeaderValueInvalid_Returns401()
    {
        // Arrange
        var securityOptions = new SecurityOptions
        {
            AuthHeaderName = "X-Auth-Token",
            AuthHeaderValue = "secret-token"
        };
        var optionsMock = Options.Create(securityOptions);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Request.Headers["X-Auth-Token"] = "wrong-token";
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        var middleware = new McpSecurityMiddleware(next, optionsMock, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(context.Response.Body).ReadToEnd();
        Assert.Contains("Invalid authentication header", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_WhenAuthHeaderValid_AllowsRequest()
    {
        // Arrange
        var securityOptions = new SecurityOptions
        {
            AuthHeaderName = "X-Auth-Token",
            AuthHeaderValue = "secret-token"
        };
        var optionsMock = Options.Create(securityOptions);
        var context = new DefaultHttpContext();
        context.Request.Headers["X-Auth-Token"] = "secret-token";
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        var middleware = new McpSecurityMiddleware(next, optionsMock, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(nextCalled);
        Assert.NotEqual(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
    }
}
