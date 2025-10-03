using BookStackMcpServer.Models;

namespace BookStackMcpServer.Tests.Models;

public class SecurityOptionsTests
{
    [Fact]
    public void SectionName_ReturnsCorrectValue()
    {
        // Assert
        Assert.Equal("Security", SecurityOptions.SectionName);
    }

    [Fact]
    public void Properties_CanBeSet()
    {
        // Arrange
        var options = new SecurityOptions
        {
            AuthHeaderName = "X-Auth-Token",
            AuthHeaderValue = "secret-value"
        };

        // Assert
        Assert.Equal("X-Auth-Token", options.AuthHeaderName);
        Assert.Equal("secret-value", options.AuthHeaderValue);
    }

    [Fact]
    public void DefaultValues_AreNull()
    {
        // Arrange
        var options = new SecurityOptions();

        // Assert
        Assert.Null(options.AuthHeaderName);
        Assert.Null(options.AuthHeaderValue);
    }
}
