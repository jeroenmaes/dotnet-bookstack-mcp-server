namespace BookStackMcpServer.Models;

public class ThrottlingOptions
{
    public const string SectionName = "Throttling";
    
    public bool Enabled { get; set; } = true;
    public int PermitLimit { get; set; } = 100;
    public int WindowSeconds { get; set; } = 60;
    public int QueueLimit { get; set; } = 0;
}
