namespace BookStackMcpServer.Models;

public class BookStackOptions
{
    public const string SectionName = "BookStack";
    
    public string BaseUrl { get; set; } = string.Empty;
    public string TokenId { get; set; } = string.Empty;
    public string TokenSecret { get; set; } = string.Empty;
}