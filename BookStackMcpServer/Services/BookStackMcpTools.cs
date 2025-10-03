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
        try
        {
            _logger.LogInformation("Listing books with offset={Offset}, count={Count}", offset, count);
            var listing = new ListingOptions(offset: offset, count: count);
            var response = await _client.ListBooksAsync(listing);
            _logger.LogDebug("Retrieved {Count} books", response.data.Length);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list books with offset={Offset}, count={Count}", offset, count);
            return JsonSerializer.Serialize(new { error = "Failed to list books", message = ex.Message }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Get book details by ID")]
    [McpServerTool]
    public async Task<string> GetBookAsync(int id)
    {
        try
        {
            _logger.LogInformation("Getting book with ID={BookId}", id);
            var book = await _client.ReadBookAsync(id);
            return JsonSerializer.Serialize(book, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get book with ID={BookId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to get book", message = ex.Message, bookId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Create a new book")]
    [McpServerTool]
    public async Task<string> CreateBookAsync(string name, string? description = null)
    {
        try
        {
            _logger.LogInformation("Creating book with name='{BookName}'", name);
            var args = new CreateBookArgs(name, description);
            var result = await _client.CreateBookAsync(args);
            _logger.LogInformation("Book created successfully with ID={BookId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create book with name='{BookName}'", name);
            return JsonSerializer.Serialize(new { error = "Failed to create book", message = ex.Message, bookName = name }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Delete a book")]
    [McpServerTool]
    public async Task<string> DeleteBookAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting book with ID={BookId}", id);
            await _client.DeleteBookAsync(id);
            _logger.LogInformation("Book deleted successfully with ID={BookId}", id);
            return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete book with ID={BookId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to delete book", message = ex.Message, bookId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Chapters management - simplified version
    [Description("List all chapters")]
    [McpServerTool]
    public async Task<string> ListChaptersAsync(int offset = 0, int count = 50)
    {
        try
        {
            _logger.LogInformation("Listing chapters with offset={Offset}, count={Count}", offset, count);
            var listing = new ListingOptions(offset: offset, count: count);
            var response = await _client.ListChaptersAsync(listing);
            _logger.LogDebug("Retrieved {Count} chapters", response.data.Length);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list chapters with offset={Offset}, count={Count}", offset, count);
            return JsonSerializer.Serialize(new { error = "Failed to list chapters", message = ex.Message }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Get chapter details by ID")]
    [McpServerTool]
    public async Task<string> GetChapterAsync(int id)
    {
        try
        {
            _logger.LogInformation("Getting chapter with ID={ChapterId}", id);
            var chapter = await _client.ReadChapterAsync(id);
            return JsonSerializer.Serialize(chapter, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get chapter with ID={ChapterId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to get chapter", message = ex.Message, chapterId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Create a new chapter")]
    [McpServerTool]
    public async Task<string> CreateChapterAsync(string name, int bookId, string? description = null, int priority = 0)
    {
        try
        {
            _logger.LogInformation("Creating chapter with name='{ChapterName}', bookId={BookId}", name, bookId);
            var args = new CreateChapterArgs(bookId, name, description, priority: priority);
            var result = await _client.CreateChapterAsync(args);
            _logger.LogInformation("Chapter created successfully with ID={ChapterId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create chapter with name='{ChapterName}', bookId={BookId}", name, bookId);
            return JsonSerializer.Serialize(new { error = "Failed to create chapter", message = ex.Message, chapterName = name, bookId }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Delete a chapter")]
    [McpServerTool]
    public async Task<string> DeleteChapterAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting chapter with ID={ChapterId}", id);
            await _client.DeleteChapterAsync(id);
            _logger.LogInformation("Chapter deleted successfully with ID={ChapterId}", id);
            return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete chapter with ID={ChapterId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to delete chapter", message = ex.Message, chapterId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Pages management - simplified version
    [Description("List all pages")]
    [McpServerTool]
    public async Task<string> ListPagesAsync(int offset = 0, int count = 50)
    {
        try
        {
            _logger.LogInformation("Listing pages with offset={Offset}, count={Count}", offset, count);
            var listing = new ListingOptions(offset: offset, count: count);
            var response = await _client.ListPagesAsync(listing);
            _logger.LogDebug("Retrieved {Count} pages", response.data.Length);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list pages with offset={Offset}, count={Count}", offset, count);
            return JsonSerializer.Serialize(new { error = "Failed to list pages", message = ex.Message }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Get page details by ID")]
    [McpServerTool]
    public async Task<string> GetPageAsync(int id)
    {
        try
        {
            _logger.LogInformation("Getting page with ID={PageId}", id);
            var page = await _client.ReadPageAsync(id);
            return JsonSerializer.Serialize(page, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get page with ID={PageId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to get page", message = ex.Message, pageId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Create a new page")]
    [McpServerTool]
    public async Task<string> CreatePageAsync(string name, string content, int? bookId = null, int? chapterId = null)
    {
        try
        {
            _logger.LogInformation("Creating page with name='{PageName}', bookId={BookId}, chapterId={ChapterId}", name, bookId, chapterId);
            var args = new CreatePageArgs(name, bookId, chapterId, html: content);
            var result = await _client.CreatePageAsync(args);
            _logger.LogInformation("Page created successfully with ID={PageId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create page with name='{PageName}', bookId={BookId}, chapterId={ChapterId}", name, bookId, chapterId);
            return JsonSerializer.Serialize(new { error = "Failed to create page", message = ex.Message, pageName = name, bookId, chapterId }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Delete a page")]
    [McpServerTool]
    public async Task<string> DeletePageAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting page with ID={PageId}", id);
            await _client.DeletePageAsync(id);
            _logger.LogInformation("Page deleted successfully with ID={PageId}", id);
            return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete page with ID={PageId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to delete page", message = ex.Message, pageId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Shelves management - simplified version
    [Description("List all shelves")]
    [McpServerTool]
    public async Task<string> ListShelvesAsync(int offset = 0, int count = 50)
    {
        try
        {
            _logger.LogInformation("Listing shelves with offset={Offset}, count={Count}", offset, count);
            var listing = new ListingOptions(offset: offset, count: count);
            var response = await _client.ListShelvesAsync(listing);
            _logger.LogDebug("Retrieved {Count} shelves", response.data.Length);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list shelves with offset={Offset}, count={Count}", offset, count);
            return JsonSerializer.Serialize(new { error = "Failed to list shelves", message = ex.Message }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Get shelf details by ID")]
    [McpServerTool]
    public async Task<string> GetShelfAsync(int id)
    {
        try
        {
            _logger.LogInformation("Getting shelf with ID={ShelfId}", id);
            var shelf = await _client.ReadShelfAsync(id);
            return JsonSerializer.Serialize(shelf, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get shelf with ID={ShelfId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to get shelf", message = ex.Message, shelfId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Create a new shelf")]
    [McpServerTool]
    public async Task<string> CreateShelfAsync(string name, string? description = null)
    {
        try
        {
            _logger.LogInformation("Creating shelf with name='{ShelfName}'", name);
            var args = new CreateShelfArgs(name, description);
            var result = await _client.CreateShelfAsync(args);
            _logger.LogInformation("Shelf created successfully with ID={ShelfId}", result.id);
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create shelf with name='{ShelfName}'", name);
            return JsonSerializer.Serialize(new { error = "Failed to create shelf", message = ex.Message, shelfName = name }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Delete a shelf")]
    [McpServerTool]
    public async Task<string> DeleteShelfAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting shelf with ID={ShelfId}", id);
            await _client.DeleteShelfAsync(id);
            _logger.LogInformation("Shelf deleted successfully with ID={ShelfId}", id);
            return JsonSerializer.Serialize(new { success = true }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete shelf with ID={ShelfId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to delete shelf", message = ex.Message, shelfId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Users management - simplified version
    [Description("List all users")]
    [McpServerTool]
    public async Task<string> ListUsersAsync(int offset = 0, int count = 50)
    {
        try
        {
            _logger.LogInformation("Listing users with offset={Offset}, count={Count}", offset, count);
            var listing = new ListingOptions(offset: offset, count: count);
            var response = await _client.ListUsersAsync(listing);
            _logger.LogDebug("Retrieved {Count} users", response.data.Length);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list users with offset={Offset}, count={Count}", offset, count);
            return JsonSerializer.Serialize(new { error = "Failed to list users", message = ex.Message }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    
    [Description("Get user details by ID")]
    [McpServerTool]
    public async Task<string> GetUserAsync(int id)
    {
        try
        {
            _logger.LogInformation("Getting user with ID={UserId}", id);
            var user = await _client.ReadUserAsync(id);
            return JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user with ID={UserId}", id);
            return JsonSerializer.Serialize(new { error = "Failed to get user", message = ex.Message, userId = id }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    // Search functionality
    [Description("Search across all BookStack content (books, chapters, pages)")]
    [McpServerTool]
    public async Task<string> SearchAllAsync(string query, int offset = 0, int count = 50)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search all content with query='{Query}'", query);
            return JsonSerializer.Serialize(new { error = "Failed to search content", message = ex.Message, query }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Search for books by name or description")]
    [McpServerTool]
    public async Task<string> SearchBooksAsync(string query, int offset = 0, int count = 50)
    {
        try
        {
            _logger.LogInformation("Searching books with query='{Query}', offset={Offset}, count={Count}", query, offset, count);
            var page = offset / count + 1;
            var args = new SearchArgs(query, count, page);
            var response = await _client.SearchAsync(args);
            
            var results = new
            {
                query = query,
                total = response.books().Count(),
                data = response.books().ToList()
            };
            
            _logger.LogDebug("Books search returned {Total} results", results.total);
            return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search books with query='{Query}'", query);
            return JsonSerializer.Serialize(new { error = "Failed to search books", message = ex.Message, query }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Search for chapters by name or description")]
    [McpServerTool]
    public async Task<string> SearchChaptersAsync(string query, int offset = 0, int count = 50)
    {
        try
        {
            _logger.LogInformation("Searching chapters with query='{Query}', offset={Offset}, count={Count}", query, offset, count);
            var page = offset / count + 1;
            var args = new SearchArgs(query, count, page);
            var response = await _client.SearchAsync(args);
            
            var results = new
            {
                query = query,
                total = response.chapters().Count(),
                data = response.chapters().ToList()
            };
            
            _logger.LogDebug("Chapters search returned {Total} results", results.total);
            return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search chapters with query='{Query}'", query);
            return JsonSerializer.Serialize(new { error = "Failed to search chapters", message = ex.Message, query }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Search for pages by name or content")]
    [McpServerTool]
    public async Task<string> SearchPagesAsync(string query, int offset = 0, int count = 50)
    {
        try
        {
            _logger.LogInformation("Searching pages with query='{Query}', offset={Offset}, count={Count}", query, offset, count);
            var page = offset / count + 1;
            var args = new SearchArgs(query, count, page);
            var response = await _client.SearchAsync(args);
            
            var results = new
            {
                query = query,
                total = response.pages().Count(),
                data = response.pages().ToList()
            };
            
            _logger.LogDebug("Pages search returned {Total} results", results.total);
            return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search pages with query='{Query}'", query);
            return JsonSerializer.Serialize(new { error = "Failed to search pages", message = ex.Message, query }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Search for shelves by name or description")]
    [McpServerTool]
    public async Task<string> SearchShelvesAsync(string query, int offset = 0, int count = 50)
    {
        try
        {
            _logger.LogInformation("Searching shelves with query='{Query}', offset={Offset}, count={Count}", query, offset, count);
            var page = offset / count + 1;
            var args = new SearchArgs(query, count, page);
            var response = await _client.SearchAsync(args);
            
            var results = new
            {
                query = query,
                total = response.shelves().Count(),
                data = response.shelves().ToList()
            };
            
            _logger.LogDebug("Shelves search returned {Total} results", results.total);
            return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search shelves with query='{Query}'", query);
            return JsonSerializer.Serialize(new { error = "Failed to search shelves", message = ex.Message, query }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Search for users by name or email")]
    [McpServerTool]
    public async Task<string> SearchUsersAsync(string query, int offset = 0, int count = 50)
    {
        try
        {
            _logger.LogInformation("Searching users with query='{Query}', offset={Offset}, count={Count}", query, offset, count);
            // Note: The new API's search doesn't include users, so we use list with filters
            var listing = new ListingOptions(offset: offset, count: count, filters: new[] { new Filter("name:like", $"%{query}%") });
            var response = await _client.ListUsersAsync(listing);
            _logger.LogDebug("Users search returned {Count} results", response.data.Length);
            return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search users with query='{Query}'", query);
            return JsonSerializer.Serialize(new { error = "Failed to search users", message = ex.Message, query }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    [Description("Advanced search with custom filters")]
    [McpServerTool]
    public async Task<string> AdvancedSearchAsync(string entityType, string field, string value, string operatorType = "like", int offset = 0, int count = 50)
    {
        try
        {
            _logger.LogInformation("Advanced search for entityType='{EntityType}', field='{Field}', value='{Value}', operator='{Operator}'", entityType, field, value, operatorType);
            var filter = new Filter($"{field}:{operatorType}", value);
            var listing = new ListingOptions(offset: offset, count: count, filters: new[] { filter });
            
            // Determine entity type and search accordingly
            switch (entityType.ToLower())
            {
                case "book":
                case "books":
                    var bookResponse = await _client.ListBooksAsync(listing);
                    _logger.LogDebug("Advanced search returned {Count} books", bookResponse.data.Length);
                    return JsonSerializer.Serialize(bookResponse, new JsonSerializerOptions { WriteIndented = true });
                    
                case "chapter":
                case "chapters":
                    var chapterResponse = await _client.ListChaptersAsync(listing);
                    _logger.LogDebug("Advanced search returned {Count} chapters", chapterResponse.data.Length);
                    return JsonSerializer.Serialize(chapterResponse, new JsonSerializerOptions { WriteIndented = true });
                    
                case "page":
                case "pages":
                    var pageResponse = await _client.ListPagesAsync(listing);
                    _logger.LogDebug("Advanced search returned {Count} pages", pageResponse.data.Length);
                    return JsonSerializer.Serialize(pageResponse, new JsonSerializerOptions { WriteIndented = true });
                    
                case "shelf":
                case "shelves":
                    var shelfResponse = await _client.ListShelvesAsync(listing);
                    _logger.LogDebug("Advanced search returned {Count} shelves", shelfResponse.data.Length);
                    return JsonSerializer.Serialize(shelfResponse, new JsonSerializerOptions { WriteIndented = true });
                    
                case "user":
                case "users":
                    var userResponse = await _client.ListUsersAsync(listing);
                    _logger.LogDebug("Advanced search returned {Count} users", userResponse.data.Length);
                    return JsonSerializer.Serialize(userResponse, new JsonSerializerOptions { WriteIndented = true });
                    
                default:
                    _logger.LogWarning("Unknown entity type requested: {EntityType}", entityType);
                    return JsonSerializer.Serialize(new { error = $"Unknown entity type: {entityType}. Supported types: book, chapter, page, shelf, user" }, new JsonSerializerOptions { WriteIndented = true });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to perform advanced search for entityType='{EntityType}', field='{Field}'", entityType, field);
            return JsonSerializer.Serialize(new { error = "Failed to perform advanced search", message = ex.Message, entityType, field }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}