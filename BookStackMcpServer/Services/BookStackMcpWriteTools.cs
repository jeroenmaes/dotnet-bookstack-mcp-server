using BookStackApiClient;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace BookStackMcpServer.Services;

[McpServerToolType]
public class BookStackMcpWriteTools
{
    private readonly BookStackClient _client;
    private readonly ILogger<BookStackMcpWriteTools> _logger;

    public BookStackMcpWriteTools(BookStackClient client, ILogger<BookStackMcpWriteTools> logger)
    {
        _client = client;
        _logger = logger;
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
}
