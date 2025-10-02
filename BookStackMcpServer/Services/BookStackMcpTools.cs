using BookStackApiClient;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace BookStackMcpServer.Services;

[McpServerToolType]
public class BookStackMcpTools
{
    private readonly BookStackClient _client;
    private readonly ILogger<BookStackMcpTools> _logger;

    public BookStackMcpTools(BookStackClient client, ILogger<BookStackMcpTools> logger)
    {
        _client = client;
        _logger = logger;
    }

    // Books management - simplified version
    [Description("List all books")]
    [McpServerTool]
    public async Task<string> ListBooksAsync(int offset = 0, int count = 50)
    {
        _logger.LogInformation("Listing books with offset={Offset}, count={Count}", offset, count);
        var listing = new ListingOptions(offset: offset, count: count);
        var response = await _client.ListBooksAsync(listing);
        _logger.LogDebug("Retrieved {Count} books", response.data.Length);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Get book details by ID")]
    [McpServerTool]
    public async Task<string> GetBookAsync(int id)
    {
        _logger.LogInformation("Getting book with ID={BookId}", id);
        var book = await _client.ReadBookAsync(id);
        return JsonSerializer.Serialize(book, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Create a new book")]
    [McpServerTool]
    public async Task<string> CreateBookAsync(string name, string? description = null)
    {
        _logger.LogInformation("Creating book with name='{BookName}'", name);
        var args = new CreateBookArgs(name, description);
        var result = await _client.CreateBookAsync(args);
        _logger.LogInformation("Book created successfully with ID={BookId}", result.id);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Delete a book")]
    [McpServerTool]
    public async Task<string> DeleteBookAsync(int id)
    {
        _logger.LogInformation("Deleting book with ID={BookId}", id);
        await _client.DeleteBookAsync(id);
        _logger.LogInformation("Book deleted successfully with ID={BookId}", id);
        return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Chapters management - simplified version
    [Description("List all chapters")]
    [McpServerTool]
    public async Task<string> ListChaptersAsync(int offset = 0, int count = 50)
    {
        var listing = new ListingOptions(offset: offset, count: count);
        var response = await _client.ListChaptersAsync(listing);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Get chapter details by ID")]
    [McpServerTool]
    public async Task<string> GetChapterAsync(int id)
    {
        var chapter = await _client.ReadChapterAsync(id);
        return JsonSerializer.Serialize(chapter, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Create a new chapter")]
    [McpServerTool]
    public async Task<string> CreateChapterAsync(string name, int bookId, string? description = null, int priority = 0)
    {
        var args = new CreateChapterArgs(bookId, name, description, priority: priority);
        var result = await _client.CreateChapterAsync(args);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Delete a chapter")]
    [McpServerTool]
    public async Task<string> DeleteChapterAsync(int id)
    {
        await _client.DeleteChapterAsync(id);
        return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Pages management - simplified version
    [Description("List all pages")]
    [McpServerTool]
    public async Task<string> ListPagesAsync(int offset = 0, int count = 50)
    {
        var listing = new ListingOptions(offset: offset, count: count);
        var response = await _client.ListPagesAsync(listing);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Get page details by ID")]
    [McpServerTool]
    public async Task<string> GetPageAsync(int id)
    {
        var page = await _client.ReadPageAsync(id);
        return JsonSerializer.Serialize(page, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Create a new page")]
    [McpServerTool]
    public async Task<string> CreatePageAsync(string name, string content, int? bookId = null, int? chapterId = null)
    {
        var args = new CreatePageArgs(name, bookId, chapterId, html: content);
        var result = await _client.CreatePageAsync(args);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Delete a page")]
    [McpServerTool]
    public async Task<string> DeletePageAsync(int id)
    {
        await _client.DeletePageAsync(id);
        return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Shelves management - simplified version
    [Description("List all shelves")]
    [McpServerTool]
    public async Task<string> ListShelvesAsync(int offset = 0, int count = 50)
    {
        var listing = new ListingOptions(offset: offset, count: count);
        var response = await _client.ListShelvesAsync(listing);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Get shelf details by ID")]
    [McpServerTool]
    public async Task<string> GetShelfAsync(int id)
    {
        var shelf = await _client.ReadShelfAsync(id);
        return JsonSerializer.Serialize(shelf, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Create a new shelf")]
    [McpServerTool]
    public async Task<string> CreateShelfAsync(string name, string? description = null)
    {
        var args = new CreateShelfArgs(name, description);
        var result = await _client.CreateShelfAsync(args);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Delete a shelf")]
    [McpServerTool]
    public async Task<string> DeleteShelfAsync(int id)
    {
        await _client.DeleteShelfAsync(id);
        return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Users management - simplified version
    [Description("List all users")]
    [McpServerTool]
    public async Task<string> ListUsersAsync(int offset = 0, int count = 50)
    {
        var listing = new ListingOptions(offset: offset, count: count);
        var response = await _client.ListUsersAsync(listing);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Get user details by ID")]
    [McpServerTool]
    public async Task<string> GetUserAsync(int id)
    {
        var user = await _client.ReadUserAsync(id);
        return JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
    }

    // Search functionality
    [Description("Search across all BookStack content (books, chapters, pages)")]
    [McpServerTool]
    public async Task<string> SearchAllAsync(string query, int offset = 0, int count = 50)
    {
        _logger.LogInformation("Searching all content with query='{Query}', offset={Offset}, count={Count}", query, offset, count);
        var page = offset / count + 1;
        var args = new SearchArgs(query, count, page);
        var response = await _client.SearchAsync(args);
        
        var results = new
        {
            query = query,
            total = response.total,
            books = response.books().ToList(),
            chapters = response.chapters().ToList(),
            pages = response.pages().ToList(),
            shelves = response.shelves().ToList()
        };
        
        _logger.LogDebug("Search returned {Total} total results", response.total);
        return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
    }

    [Description("Search for books by name or description")]
    [McpServerTool]
    public async Task<string> SearchBooksAsync(string query, int offset = 0, int count = 50)
    {
        var page = offset / count + 1;
        var args = new SearchArgs(query, count, page);
        var response = await _client.SearchAsync(args);
        
        var results = new
        {
            query = query,
            total = response.books().Count(),
            data = response.books().ToList()
        };
        
        return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
    }

    [Description("Search for chapters by name or description")]
    [McpServerTool]
    public async Task<string> SearchChaptersAsync(string query, int offset = 0, int count = 50)
    {
        var page = offset / count + 1;
        var args = new SearchArgs(query, count, page);
        var response = await _client.SearchAsync(args);
        
        var results = new
        {
            query = query,
            total = response.chapters().Count(),
            data = response.chapters().ToList()
        };
        
        return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
    }

    [Description("Search for pages by name or content")]
    [McpServerTool]
    public async Task<string> SearchPagesAsync(string query, int offset = 0, int count = 50)
    {
        var page = offset / count + 1;
        var args = new SearchArgs(query, count, page);
        var response = await _client.SearchAsync(args);
        
        var results = new
        {
            query = query,
            total = response.pages().Count(),
            data = response.pages().ToList()
        };
        
        return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
    }

    [Description("Search for shelves by name or description")]
    [McpServerTool]
    public async Task<string> SearchShelvesAsync(string query, int offset = 0, int count = 50)
    {
        var page = offset / count + 1;
        var args = new SearchArgs(query, count, page);
        var response = await _client.SearchAsync(args);
        
        var results = new
        {
            query = query,
            total = response.shelves().Count(),
            data = response.shelves().ToList()
        };
        
        return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
    }

    [Description("Search for users by name or email")]
    [McpServerTool]
    public async Task<string> SearchUsersAsync(string query, int offset = 0, int count = 50)
    {
        // Note: The new API's search doesn't include users, so we use list with filters
        var listing = new ListingOptions(offset: offset, count: count, filters: new[] { new Filter("name:like", $"%{query}%") });
        var response = await _client.ListUsersAsync(listing);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }

    [Description("Advanced search with custom filters")]
    [McpServerTool]
    public async Task<string> AdvancedSearchAsync(string entityType, string field, string value, string operatorType = "like", int offset = 0, int count = 50)
    {
        var filter = new Filter($"{field}:{operatorType}", value);
        var listing = new ListingOptions(offset: offset, count: count, filters: new[] { filter });
        
        // Determine entity type and search accordingly
        switch (entityType.ToLower())
        {
            case "book":
            case "books":
                var bookResponse = await _client.ListBooksAsync(listing);
                return JsonSerializer.Serialize(bookResponse, new JsonSerializerOptions { WriteIndented = true });
                
            case "chapter":
            case "chapters":
                var chapterResponse = await _client.ListChaptersAsync(listing);
                return JsonSerializer.Serialize(chapterResponse, new JsonSerializerOptions { WriteIndented = true });
                
            case "page":
            case "pages":
                var pageResponse = await _client.ListPagesAsync(listing);
                return JsonSerializer.Serialize(pageResponse, new JsonSerializerOptions { WriteIndented = true });
                
            case "shelf":
            case "shelves":
                var shelfResponse = await _client.ListShelvesAsync(listing);
                return JsonSerializer.Serialize(shelfResponse, new JsonSerializerOptions { WriteIndented = true });
                
            case "user":
            case "users":
                var userResponse = await _client.ListUsersAsync(listing);
                return JsonSerializer.Serialize(userResponse, new JsonSerializerOptions { WriteIndented = true });
                
            default:
                return JsonSerializer.Serialize(new { error = $"Unknown entity type: {entityType}. Supported types: book, chapter, page, shelf, user" }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}