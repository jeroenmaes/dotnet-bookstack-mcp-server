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

### Search Functionality
- `search_all` - Search across all BookStack content (books, chapters, pages)
- `search_books` - Search for books by name or description
- `search_chapters` - Search for chapters by name or description  
- `search_pages` - Search for pages by name or content
- `search_shelves` - Search for shelves by name or description
- `search_users` - Search for users by name or email
- `advanced_search` - Advanced search with custom filters and operators

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

### Docker Deployment

The application includes Docker support for easy deployment:

#### Using Docker Compose (Recommended)

1. Copy the environment template:
```bash
cp .env.example .env
```

2. Edit `.env` file with your BookStack configuration:
```bash
BOOKSTACK_BASE_URL=https://your-bookstack-instance.com
BOOKSTACK_TOKEN_ID=your-token-id
BOOKSTACK_TOKEN_SECRET=your-token-secret
```

3. Build and run with Docker Compose:
```bash
docker-compose up -d
```

#### Using Docker directly

1. Build the Docker image:
```bash
docker build -t bookstack-mcp-server .
```

2. Run the container:
```bash
docker run -d \
  -p 5230:5230 \
  -e BookStack__BaseUrl=https://your-bookstack-instance.com \
  -e BookStack__TokenId=your-token-id \
  -e BookStack__TokenSecret=your-token-secret \
  --name bookstack-mcp-server \
  bookstack-mcp-server
```

#### GitHub Container Registry

Pre-built images are available from GitHub Container Registry:

```bash
docker pull ghcr.io/jeroenmaes/dotnet-bookstack-mcp-server:latest
```

## API Endpoints (Development)

While the full MCP protocol implementation is in progress, the server provides HTTP endpoints for testing:

- `GET /tools` - List all available MCP tools with their parameters
- `GET /test` - Test BookStack API connectivity
- `POST /invoke/{toolName}` - Invoke a specific tool with parameters

### Health Check Endpoints

The server provides ASP.NET Core health check endpoints for monitoring:

- `GET /health` - Comprehensive health check with detailed status information
- `GET /health/ready` - Readiness check (includes BookStack API dependency check)
- `GET /health/live` - Liveness check (returns 200 if the application is running)

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

Search for books:
```bash
curl -X POST http://localhost:5230/invoke/search_books \
  -H "Content-Type: application/json" \
  -d '{"query": "tutorial", "offset": 0, "count": 10}'
```

Advanced search example:
```bash
curl -X POST http://localhost:5230/invoke/advanced_search \
  -H "Content-Type: application/json" \
  -d '{"entityType": "page", "field": "name", "value": "guide", "operatorType": "Like"}'
```

Check application health:
```bash
# Liveness check (app is running)
curl http://localhost:5230/health/live

# Readiness check (app is ready to serve requests, including BookStack API)
curl http://localhost:5230/health/ready

# Detailed health information
curl http://localhost:5230/health
```

## BookStack API Setup

1. In your BookStack instance, go to Settings → Users → API Tokens
2. Create a new API token with the required permissions
3. Use the Token ID and Token Secret in your configuration

## Dependencies

- **ASP.NET Core 8** - Web framework
- **ModelContextProtocol.AspNetCore** (0.4.0-preview.1) - MCP server implementation
- **BookStackApi** (1.3.0) - BookStack API client library
- **Microsoft.Extensions.Diagnostics.HealthChecks** (9.0.9) - Health checks support

## Development Status

- [x] ASP.NET Core 8 project structure
- [x] BookStack API integration
- [x] MCP tool definitions for all major BookStack entities
- [x] HTTP endpoints for testing
- [x] Health checks with BookStack API dependency check
- [ ] Full MCP protocol transport implementation
- [ ] Authentication and authorization
- [ ] Error handling improvements
- [ ] Unit tests

## License

MIT License - see LICENSE file for details.