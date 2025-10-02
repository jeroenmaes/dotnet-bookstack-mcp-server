namespace BookStackMcpServer.Models;

public class SecurityOptions
{
    public const string SectionName = "Security";
    
    public string? AuthHeaderName { get; set; }
    public string? AuthHeaderValue { get; set; }
}
