using BookStackApi;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace BookStackMcpServer.Services;

public class BookStackMcpTools
{
    private readonly ApiService _apiService;

    public BookStackMcpTools(ApiService apiService)
    {
        _apiService = apiService;
    }

    // Books management
    [Description("List all books")]
    [McpServerTool(Name = "list_books")]
    public async Task<string> ListBooksAsync(int offset = 0, int count = 50, string? filter = null, string? sort = null)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        if (!string.IsNullOrEmpty(filter)) parameters.Filters = new[] { new Filter { Field = "name", Value = filter } };
        if (!string.IsNullOrEmpty(sort)) parameters.Sort = sort;

        var response = await _apiService.GetListAsync<Book>(parameters);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Get book details by ID")]
    [McpServerTool(Name = "get_book")]
    public async Task<string> GetBookAsync(int id)
    {
        var book = await _apiService.GetDetailsAsync<BookDetails>(id);
        return JsonSerializer.Serialize(book, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Create a new book")]
    [McpServerTool(Name = "create_book")]
    public async Task<string> CreateBookAsync(string name, string? description = null, int? imageId = null)
    {
        var book = new Book
        {
            Name = name,
            Description = description ?? string.Empty,
            ImageId = imageId ?? 0
        };
        
        var result = await _apiService.PostAsync(book);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Update an existing book")]
    [McpServerTool(Name = "update_book")]
    public async Task<string> UpdateBookAsync(int id, string? name = null, string? description = null, int? imageId = null)
    {
        var existingBook = await _apiService.GetDetailsAsync<BookDetails>(id);
        
        var book = new Book
        {
            Id = id,
            Name = name ?? existingBook.Name,
            Description = description ?? existingBook.Description,
            ImageId = imageId ?? existingBook.ImageId
        };
        
        var result = await _apiService.PutAsync(book);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Delete a book")]
    [McpServerTool(Name = "delete_book")]
    public async Task<string> DeleteBookAsync(int id)
    {
        var result = await _apiService.DeleteAsync<Book>(id);
        return JsonSerializer.Serialize(new { success = result }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Chapters management
    [Description("List all chapters")]
    [McpServerTool(Name = "list_chapters")]
    public async Task<string> ListChaptersAsync(int offset = 0, int count = 50, string? filter = null, string? sort = null)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        if (!string.IsNullOrEmpty(filter)) parameters.Filters = new[] { new Filter { Field = "name", Value = filter } };
        if (!string.IsNullOrEmpty(sort)) parameters.Sort = sort;

        var response = await _apiService.GetListAsync<Chapter>(parameters);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Get chapter details by ID")]
    [McpServerTool(Name = "get_chapter")]
    public async Task<string> GetChapterAsync(int id)
    {
        var chapter = await _apiService.GetDetailsAsync<ChapterDetails>(id);
        return JsonSerializer.Serialize(chapter, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Create a new chapter")]
    [McpServerTool(Name = "create_chapter")]
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
    
    [Description("Update an existing chapter")]
    [McpServerTool(Name = "update_chapter")]
    public async Task<string> UpdateChapterAsync(int id, string? name = null, int? bookId = null, string? description = null, int? priority = null)
    {
        var existingChapter = await _apiService.GetDetailsAsync<ChapterDetails>(id);
        
        var chapter = new Chapter
        {
            Id = id,
            Name = name ?? existingChapter.Name,
            BookId = bookId ?? existingChapter.BookId,
            Description = description ?? existingChapter.Description,
            Priority = priority ?? existingChapter.Priority
        };
        
        var result = await _apiService.PutAsync(chapter);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Delete a chapter")]
    [McpServerTool(Name = "delete_chapter")]
    public async Task<string> DeleteChapterAsync(int id)
    {
        var result = await _apiService.DeleteAsync<Chapter>(id);
        return JsonSerializer.Serialize(new { success = result }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Pages management
    [Description("List all pages")]
    [McpServerTool(Name = "list_pages")]
    public async Task<string> ListPagesAsync(int offset = 0, int count = 50, string? filter = null, string? sort = null)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        if (!string.IsNullOrEmpty(filter)) parameters.Filters = new[] { new Filter { Field = "name", Value = filter } };
        if (!string.IsNullOrEmpty(sort)) parameters.Sort = sort;

        var response = await _apiService.GetListAsync<Page>(parameters);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Get page details by ID")]
    [McpServerTool(Name = "get_page")]
    public async Task<string> GetPageAsync(int id)
    {
        var page = await _apiService.GetDetailsAsync<PageDetails>(id);
        return JsonSerializer.Serialize(page, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Create a new page")]
    [McpServerTool(Name = "create_page")]
    public async Task<string> CreatePageAsync(string name, string html, int? bookId = null, int? chapterId = null, int priority = 0)
    {
        var page = new Page
        {
            Name = name,
            Html = html,
            BookId = bookId ?? 0,
            ChapterId = chapterId ?? 0,
            Priority = priority
        };
        
        var result = await _apiService.PostAsync(page);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Update an existing page")]
    [McpServerTool(Name = "update_page")]
    public async Task<string> UpdatePageAsync(int id, string? name = null, string? html = null, int? bookId = null, int? chapterId = null, int? priority = null)
    {
        var existingPage = await _apiService.GetDetailsAsync<PageDetails>(id);
        
        var page = new Page
        {
            Id = id,
            Name = name ?? existingPage.Name,
            Html = html ?? existingPage.Html,
            BookId = bookId ?? existingPage.BookId,
            ChapterId = chapterId ?? existingPage.ChapterId,
            Priority = priority ?? existingPage.Priority
        };
        
        var result = await _apiService.PutAsync(page);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Delete a page")]
    [McpServerTool(Name = "delete_page")]
    public async Task<string> DeletePageAsync(int id)
    {
        var result = await _apiService.DeleteAsync<Page>(id);
        return JsonSerializer.Serialize(new { success = result }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Shelves management
    [Description("List all shelves")]
    [McpServerTool(Name = "list_shelves")]
    public async Task<string> ListShelvesAsync(int offset = 0, int count = 50, string? filter = null, string? sort = null)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        if (!string.IsNullOrEmpty(filter)) parameters.Filters = new[] { new Filter { Field = "name", Value = filter } };
        if (!string.IsNullOrEmpty(sort)) parameters.Sort = sort;

        var response = await _apiService.GetListAsync<Shelf>(parameters);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Get shelf details by ID")]
    [McpServerTool(Name = "get_shelf")]
    public async Task<string> GetShelfAsync(int id)
    {
        var shelf = await _apiService.GetDetailsAsync<ShelfDetails>(id);
        return JsonSerializer.Serialize(shelf, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Create a new shelf")]
    [McpServerTool(Name = "create_shelf")]
    public async Task<string> CreateShelfAsync(string name, string? description = null, int? imageId = null)
    {
        var shelf = new Shelf
        {
            Name = name,
            Description = description ?? string.Empty,
            ImageId = imageId ?? 0
        };
        
        var result = await _apiService.PostAsync(shelf);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Update an existing shelf")]
    [McpServerTool(Name = "update_shelf")]
    public async Task<string> UpdateShelfAsync(int id, string? name = null, string? description = null, int? imageId = null)
    {
        var existingShelf = await _apiService.GetDetailsAsync<ShelfDetails>(id);
        
        var shelf = new Shelf
        {
            Id = id,
            Name = name ?? existingShelf.Name,
            Description = description ?? existingShelf.Description,
            ImageId = imageId ?? existingShelf.ImageId
        };
        
        var result = await _apiService.PutAsync(shelf);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Delete a shelf")]
    [McpServerTool(Name = "delete_shelf")]
    public async Task<string> DeleteShelfAsync(int id)
    {
        var result = await _apiService.DeleteAsync<Shelf>(id);
        return JsonSerializer.Serialize(new { success = result }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Users management
    [Description("List all users")]
    [McpServerTool(Name = "list_users")]
    public async Task<string> ListUsersAsync(int offset = 0, int count = 50, string? filter = null, string? sort = null)
    {
        var parameters = new ListParameters { Offset = offset, Count = count };
        if (!string.IsNullOrEmpty(filter)) parameters.Filters = new[] { new Filter { Field = "name", Value = filter } };
        if (!string.IsNullOrEmpty(sort)) parameters.Sort = sort;

        var response = await _apiService.GetListAsync<User>(parameters);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Get user details by ID")]
    [McpServerTool(Name = "get_user")]
    public async Task<string> GetUserAsync(int id)
    {
        var user = await _apiService.GetDetailsAsync<User>(id);
        return JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Create a new user")]
    [McpServerTool(Name = "create_user")]
    public async Task<string> CreateUserAsync(string name, string email, string? password = null, int? imageId = null)
    {
        var user = new User
        {
            Name = name,
            Email = email,
            ImageId = imageId ?? 0
        };
        
        var result = await _apiService.PostAsync(user);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Update an existing user")]
    [McpServerTool(Name = "update_user")]
    public async Task<string> UpdateUserAsync(int id, string? name = null, string? email = null, int? imageId = null)
    {
        var existingUser = await _apiService.GetDetailsAsync<User>(id);
        
        var user = new User
        {
            Id = id,
            Name = name ?? existingUser.Name,
            Email = email ?? existingUser.Email,
            ImageId = imageId ?? existingUser.ImageId
        };
        
        var result = await _apiService.PutAsync(user);
        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }
    
    [Description("Delete a user")]
    [McpServerTool(Name = "delete_user")]
    public async Task<string> DeleteUserAsync(int id)
    {
        var result = await _apiService.DeleteAsync<User>(id);
        return JsonSerializer.Serialize(new { success = result }, new JsonSerializerOptions { WriteIndented = true });
    }
}