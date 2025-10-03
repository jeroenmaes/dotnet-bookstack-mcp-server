using BookStackMcpServer.Models;

namespace BookStackMcpServer.Tests.Models;

public class BookStackOptionsTests
{
    [Fact]
    public void SectionName_ReturnsCorrectValue()
    {
        // Assert
        Assert.Equal("BookStack", BookStackOptions.SectionName);
    }

    [Fact]
    public void Properties_CanBeSet()
    {
        // Arrange
        var options = new BookStackOptions
        {
            BaseUrl = "https://example.com",
            TokenId = "test-token-id",
            TokenSecret = "test-token-secret"
        };

        // Assert
        Assert.Equal("https://example.com", options.BaseUrl);
        Assert.Equal("test-token-id", options.TokenId);
        Assert.Equal("test-token-secret", options.TokenSecret);
    }

    [Fact]
    public void DefaultValues_AreEmptyStrings()
    {
        // Arrange
        var options = new BookStackOptions();

        // Assert
        Assert.Equal(string.Empty, options.BaseUrl);
        Assert.Equal(string.Empty, options.TokenId);
        Assert.Equal(string.Empty, options.TokenSecret);
    }
}
