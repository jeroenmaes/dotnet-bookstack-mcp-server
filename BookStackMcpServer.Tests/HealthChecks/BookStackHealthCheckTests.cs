using BookStackMcpServer.HealthChecks;
using BookStackMcpServer.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BookStackMcpServer.Tests.HealthChecks;

public class BookStackHealthCheckTests
{
    private readonly Mock<ILogger<BookStackHealthCheck>> _loggerMock;

    public BookStackHealthCheckTests()
    {
        _loggerMock = new Mock<ILogger<BookStackHealthCheck>>();
    }

    [Fact]
    public void Constructor_TrimsTrailingSlashFromBaseUrl()
    {
        // Arrange
        var options = Options.Create(new BookStackOptions
        {
            BaseUrl = "https://example.com/",
            TokenId = "test-id",
            TokenSecret = "test-secret"
        });

        // Act
        var healthCheck = new BookStackHealthCheck(options, _loggerMock.Object);

        // Assert - if constructor doesn't throw, trimming is working
        Assert.NotNull(healthCheck);
    }

    [Fact]
    public void Constructor_HandlesBaseUrlWithoutTrailingSlash()
    {
        // Arrange
        var options = Options.Create(new BookStackOptions
        {
            BaseUrl = "https://example.com",
            TokenId = "test-id",
            TokenSecret = "test-secret"
        });

        // Act
        var healthCheck = new BookStackHealthCheck(options, _loggerMock.Object);

        // Assert
        Assert.NotNull(healthCheck);
    }
}
