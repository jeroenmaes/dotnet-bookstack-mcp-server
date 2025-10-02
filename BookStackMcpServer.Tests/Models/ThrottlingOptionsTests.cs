using BookStackMcpServer.Models;

namespace BookStackMcpServer.Tests.Models;

public class ThrottlingOptionsTests
{
    [Fact]
    public void SectionName_ReturnsCorrectValue()
    {
        // Assert
        Assert.Equal("Throttling", ThrottlingOptions.SectionName);
    }

    [Fact]
    public void Properties_CanBeSet()
    {
        // Arrange
        var options = new ThrottlingOptions
        {
            Enabled = false,
            PermitLimit = 200,
            WindowSeconds = 30,
            QueueLimit = 5
        };

        // Assert
        Assert.False(options.Enabled);
        Assert.Equal(200, options.PermitLimit);
        Assert.Equal(30, options.WindowSeconds);
        Assert.Equal(5, options.QueueLimit);
    }

    [Fact]
    public void DefaultValues_AreCorrect()
    {
        // Arrange
        var options = new ThrottlingOptions();

        // Assert
        Assert.True(options.Enabled);
        Assert.Equal(100, options.PermitLimit);
        Assert.Equal(60, options.WindowSeconds);
        Assert.Equal(0, options.QueueLimit);
    }
}
