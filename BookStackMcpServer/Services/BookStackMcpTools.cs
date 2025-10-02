using BookStackApi;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace BookStackMcpServer.Services;

[McpServerToolType]
public class BookStackMcpTools
{
    private readonly ApiService _apiService;

    public BookStackMcpTools(ApiService apiService)
    {
        _apiService = apiService;
    }

    // Books management - simplified version
    [Description("List all books")]
    [McpServerTool]
    public async Task<string> ListBooksAsync(int offset = 0, int count = 50)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        var response = await _apiService.GetListAsync<Book>(parameters);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Get book details by ID")]
    [McpServerTool]
    public async Task<string> GetBookAsync(int id)
    {
        var book = await _apiService.GetDetailsAsync<BookDetails>(id);
        return JsonSerializer.Serialize(book, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Create a new book")]
    [McpServerTool]
    public async Task<string> CreateBookAsync(string name, string? description = null)
    {
        var book = new Book
        {
            Name = name,
            Description = description ?? string.Empty
        };
        
        var result = await _apiService.PostAsync(book);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Delete a book")]
    [McpServerTool]
    public async Task<string> DeleteBookAsync(int id)
    {
        var result = await _apiService.DeleteAsync<Book>(id);
        return JsonSerializer.Serialize(new { success = result }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Chapters management - simplified version
    [Description("List all chapters")]
    [McpServerTool]
    public async Task<string> ListChaptersAsync(int offset = 0, int count = 50)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        var response = await _apiService.GetListAsync<Chapter>(parameters);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Get chapter details by ID")]
    [McpServerTool]
    public async Task<string> GetChapterAsync(int id)
    {
        var chapter = await _apiService.GetDetailsAsync<ChapterDetails>(id);
        return JsonSerializer.Serialize(chapter, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Create a new chapter")]
    [McpServerTool]
    public async Task<string> CreateChapterAsync(string name, int bookId, string? description = null, int priority = 0)
    {
        var chapter = new Chapter
        {
            Name = name,
            BookId = bookId,
            Description = description ?? string.Empty,
            Priority = priority
        };
        
        var result = await _apiService.PostAsync(chapter);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Delete a chapter")]
    [McpServerTool]
    public async Task<string> DeleteChapterAsync(int id)
    {
        var result = await _apiService.DeleteAsync<Chapter>(id);
        return JsonSerializer.Serialize(new { success = result }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Pages management - simplified version
    [Description("List all pages")]
    [McpServerTool]
    public async Task<string> ListPagesAsync(int offset = 0, int count = 50)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        var response = await _apiService.GetListAsync<Page>(parameters);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Get page details by ID")]
    [McpServerTool]
    public async Task<string> GetPageAsync(int id)
    {
        var page = await _apiService.GetDetailsAsync<PageDetails>(id);
        return JsonSerializer.Serialize(page, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Create a new page")]
    [McpServerTool]
    public async Task<string> CreatePageAsync(string name, string content, int? bookId = null, int? chapterId = null)
    {
        var page = new Page
        {
            Name = name,
            BookId = bookId ?? 0,
            ChapterId = chapterId ?? 0
        };
        
        var result = await _apiService.PostAsync(page);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Delete a page")]
    [McpServerTool]
    public async Task<string> DeletePageAsync(int id)
    {
        var result = await _apiService.DeleteAsync<Page>(id);
        return JsonSerializer.Serialize(new { success = result }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Shelves management - simplified version
    [Description("List all shelves")]
    [McpServerTool]
    public async Task<string> ListShelvesAsync(int offset = 0, int count = 50)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        var response = await _apiService.GetListAsync<Shelf>(parameters);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Get shelf details by ID")]
    [McpServerTool]
    public async Task<string> GetShelfAsync(int id)
    {
        var shelf = await _apiService.GetDetailsAsync<ShelfDetails>(id);
        return JsonSerializer.Serialize(shelf, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Create a new shelf")]
    [McpServerTool]
    public async Task<string> CreateShelfAsync(string name, string? description = null)
    {
        var shelf = new Shelf
        {
            Name = name,
            Description = description ?? string.Empty
        };
        
        var result = await _apiService.PostAsync(shelf);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Delete a shelf")]
    [McpServerTool]
    public async Task<string> DeleteShelfAsync(int id)
    {
        var result = await _apiService.DeleteAsync<Shelf>(id);
        return JsonSerializer.Serialize(new { success = result }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Users management - simplified version
    [Description("List all users")]
    [McpServerTool]
    public async Task<string> ListUsersAsync(int offset = 0, int count = 50)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        var response = await _apiService.GetListAsync<User>(parameters);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Get user details by ID")]
    [McpServerTool]
    public async Task<string> GetUserAsync(int id)
    {
        var user = await _apiService.GetDetailsAsync<User>(id);
        return JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
    }

    // Search functionality
    [Description("Search across all BookStack content (books, chapters, pages)")]
    [McpServerTool]
    public async Task<string> SearchAllAsync(string query, int offset = 0, int count = 50)
    {
        var results = new
        {
            query = query,
            books = await SearchBooksAsync(query, offset, count),
            chapters = await SearchChaptersAsync(query, offset, count), 
            pages = await SearchPagesAsync(query, offset, count)
        };
        
        return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
    }

    [Description("Search for books by name or description")]
    [McpServerTool]
    public async Task<string> SearchBooksAsync(string query, int offset = 0, int count = 50)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        
        // Create filters for searching in name and description
        var filters = new List<Filter>
        {
            new Filter { Field = "name", Value = query, Operator = FilterOperator.Like }
        };
        
        // Add the filters to the parameters
        var filtersArray = filters.ToArray();
        typeof(ListParameters).GetProperty("Filters")?.SetValue(parameters, filtersArray);
        
        var response = await _apiService.GetListAsync<Book>(parameters);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }

    [Description("Search for chapters by name or description")]
    [McpServerTool]
    public async Task<string> SearchChaptersAsync(string query, int offset = 0, int count = 50)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        
        var filters = new List<Filter>
        {
            new Filter { Field = "name", Value = query, Operator = FilterOperator.Like }
        };
        
        var filtersArray = filters.ToArray();
        typeof(ListParameters).GetProperty("Filters")?.SetValue(parameters, filtersArray);
        
        var response = await _apiService.GetListAsync<Chapter>(parameters);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }

    [Description("Search for pages by name or content")]
    [McpServerTool]
    public async Task<string> SearchPagesAsync(string query, int offset = 0, int count = 50)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        
        var filters = new List<Filter>
        {
            new Filter { Field = "name", Value = query, Operator = FilterOperator.Like }
        };
        
        var filtersArray = filters.ToArray();
        typeof(ListParameters).GetProperty("Filters")?.SetValue(parameters, filtersArray);
        
        var response = await _apiService.GetListAsync<Page>(parameters);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }

    [Description("Search for shelves by name or description")]
    [McpServerTool]
    public async Task<string> SearchShelvesAsync(string query, int offset = 0, int count = 50)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        
        var filters = new List<Filter>
        {
            new Filter { Field = "name", Value = query, Operator = FilterOperator.Like }
        };
        
        var filtersArray = filters.ToArray();
        typeof(ListParameters).GetProperty("Filters")?.SetValue(parameters, filtersArray);
        
        var response = await _apiService.GetListAsync<Shelf>(parameters);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }

    [Description("Search for users by name or email")]
    [McpServerTool]
    public async Task<string> SearchUsersAsync(string query, int offset = 0, int count = 50)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        
        var filters = new List<Filter>
        {
            new Filter { Field = "name", Value = query, Operator = FilterOperator.Like }
        };
        
        var filtersArray = filters.ToArray();
        typeof(ListParameters).GetProperty("Filters")?.SetValue(parameters, filtersArray);
        
        var response = await _apiService.GetListAsync<User>(parameters);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }

    [Description("Advanced search with custom filters")]
    [McpServerTool]
    public async Task<string> AdvancedSearchAsync(string entityType, string field, string value, string operatorType = "like", int offset = 0, int count = 50)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        
        // Parse the operator
        if (!Enum.TryParse<FilterOperator>(operatorType, true, out var filterOperator))
        {
            filterOperator = FilterOperator.Like;
        }
        
        var filters = new List<Filter>
        {
            new Filter { Field = field, Value = value, Operator = filterOperator }
        };
        
        var filtersArray = filters.ToArray();
        typeof(ListParameters).GetProperty("Filters")?.SetValue(parameters, filtersArray);
        
        // Determine entity type and search accordingly
        switch (entityType.ToLower())
        {
            case "book":
            case "books":
                var bookResponse = await _apiService.GetListAsync<Book>(parameters);
                return JsonSerializer.Serialize(bookResponse, new JsonSerializerOptions { WriteIndented = true });
                
            case "chapter":
            case "chapters":
                var chapterResponse = await _apiService.GetListAsync<Chapter>(parameters);
                return JsonSerializer.Serialize(chapterResponse, new JsonSerializerOptions { WriteIndented = true });
                
            case "page":
            case "pages":
                var pageResponse = await _apiService.GetListAsync<Page>(parameters);
                return JsonSerializer.Serialize(pageResponse, new JsonSerializerOptions { WriteIndented = true });
                
            case "shelf":
            case "shelves":
                var shelfResponse = await _apiService.GetListAsync<Shelf>(parameters);
                return JsonSerializer.Serialize(shelfResponse, new JsonSerializerOptions { WriteIndented = true });
                
            case "user":
            case "users":
                var userResponse = await _apiService.GetListAsync<User>(parameters);
                return JsonSerializer.Serialize(userResponse, new JsonSerializerOptions { WriteIndented = true });
                
            default:
                return JsonSerializer.Serialize(new { error = $"Unknown entity type: {entityType}. Supported types: book, chapter, page, shelf, user" }, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}