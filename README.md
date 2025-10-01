# BookStack MCP Server

An ASP.NET Core 8 Model Context Protocol (MCP) server that provides tools for interacting with BookStack wiki software via its API.

## Features

This MCP server implements tools for all major BookStack API endpoints:

### Books Management
- `list_books` - List all books with pagination
- `get_book` - Get detailed information about a specific book
- `create_book` - Create a new book
- `delete_book` - Delete a book

### Chapters Management  
- `list_chapters` - List all chapters with pagination
- `get_chapter` - Get detailed information about a specific chapter
- `create_chapter` - Create a new chapter in a book
- `delete_chapter` - Delete a chapter

### Pages Management
- `list_pages` - List all pages with pagination
- `get_page` - Get detailed information about a specific page
- `create_page` - Create a new page in a book or chapter
- `delete_page` - Delete a page

### Shelves Management
- `list_shelves` - List all shelves with pagination
- `get_shelf` - Get detailed information about a specific shelf
- `create_shelf` - Create a new shelf
- `delete_shelf` - Delete a shelf

### Users Management
- `list_users` - List all users with pagination
- `get_user` - Get detailed information about a specific user

## Setup

### Prerequisites
- .NET 8 SDK
- BookStack instance with API access enabled
- BookStack API token credentials

### Configuration

1. Clone the repository:
```bash
git clone https://github.com/jeroenmaes/dotnet-bookstack-mcp-server.git
cd dotnet-bookstack-mcp-server
```

2. Configure your BookStack connection in `BookStackMcpServer/appsettings.json`:
```json
{
  "BookStack": {
    "BaseUrl": "https://your-bookstack-instance.com",
    "TokenId": "your-token-id",
    "TokenSecret": "your-token-secret"
  }
}
```

3. Run the application:
```bash
cd BookStackMcpServer
dotnet run
```

## API Endpoints (Development)

While the full MCP protocol implementation is in progress, the server provides HTTP endpoints for testing:

- `GET /tools` - List all available MCP tools with their parameters
- `GET /test` - Test BookStack API connectivity
- `POST /invoke/{toolName}` - Invoke a specific tool with parameters

### Example Usage

List available tools:
```bash
curl http://localhost:5230/tools
```

Test BookStack connection:
```bash
curl http://localhost:5230/test
```

Invoke the list_books tool:
```bash
curl -X POST http://localhost:5230/invoke/list_books \
  -H "Content-Type: application/json" \
  -d '{"offset": 0, "count": 10}'
```

## BookStack API Setup

1. In your BookStack instance, go to Settings → Users → API Tokens
2. Create a new API token with the required permissions
3. Use the Token ID and Token Secret in your configuration

## Dependencies

- **ASP.NET Core 8** - Web framework
- **ModelContextProtocol.AspNetCore** (0.4.0-preview.1) - MCP server implementation
- **BookStackApi** (1.3.0) - BookStack API client library

## Development Status

- [x] ASP.NET Core 8 project structure
- [x] BookStack API integration
- [x] MCP tool definitions for all major BookStack entities
- [x] HTTP endpoints for testing
- [ ] Full MCP protocol transport implementation
- [ ] Authentication and authorization
- [ ] Error handling improvements
- [ ] Unit tests

## License

MIT License - see LICENSE file for details.