# BookStack MCP Server

An ASP.NET Core 9 Model Context Protocol (MCP) server that provides tools for interacting with BookStack wiki software via its API.

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
- .NET 9 SDK
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
  -p 8080:8080 \
  -e BookStack__BaseUrl=https://your-bookstack-instance.com \
  -e BookStack__TokenId=your-token-id \
  -e BookStack__TokenSecret=your-token-secret \
  --name bookstack-mcp-server \
  bookstack-mcp-server
```

#### GitHub Container Registry

Pre-built images are available from GitHub Container Registry:

```bash
# Pull the latest image
docker pull ghcr.io/jeroenmaes/dotnet-bookstack-mcp-server:latest

# Pull a specific build number
docker pull ghcr.io/jeroenmaes/dotnet-bookstack-mcp-server:build-123

# Pull a specific version tag
docker pull ghcr.io/jeroenmaes/dotnet-bookstack-mcp-server:v1.0.0
```

## MCP Protocol

The server implements the Model Context Protocol using the official C# SDK. It exposes an MCP endpoint that can be used by MCP-compatible clients to interact with BookStack.

### Optional Security

The server supports optional HTTP header-based authentication for the MCP endpoints. When configured, all requests to the MCP endpoint must include the specified authentication header with the correct value.

**Configuration:**

In `appsettings.json`:
```json
{
  "Security": {
    "AuthHeaderName": "X-MCP-Auth",
    "AuthHeaderValue": "your-secret-token"
  }
}
```

Or via environment variables:
```bash
Security__AuthHeaderName=X-MCP-Auth
Security__AuthHeaderValue=your-secret-token
```

**Docker example:**
```bash
docker run -d \
  -p 8080:8080 \
  -e BookStack__BaseUrl=https://your-bookstack-instance.com \
  -e BookStack__TokenId=your-token-id \
  -e BookStack__TokenSecret=your-token-secret \
  -e Security__AuthHeaderName=X-MCP-Auth \
  -e Security__AuthHeaderValue=your-secret-token \
  --name bookstack-mcp-server \
  bookstack-mcp-server
```

**Notes:**
- If `AuthHeaderName` or `AuthHeaderValue` is not configured, security is disabled and all requests are allowed
- Health check endpoints (`/health`, `/health/live`, `/health/ready`) are not protected by this security mechanism
- When enabled, clients must send the configured header with every MCP request

### HTTP Throttling

The server includes built-in HTTP rate limiting to protect against abuse and excessive requests. Rate limiting is applied per IP address using a fixed window algorithm.

**Configuration:**

In `appsettings.json`:
```json
{
  "Throttling": {
    "Enabled": true,
    "PermitLimit": 100,
    "WindowSeconds": 60,
    "QueueLimit": 0
  }
}
```

Or via environment variables:
```bash
Throttling__Enabled=true
Throttling__PermitLimit=100
Throttling__WindowSeconds=60
Throttling__QueueLimit=0
```

**Docker example:**
```bash
docker run -d \
  -p 8080:8080 \
  -e BookStack__BaseUrl=https://your-bookstack-instance.com \
  -e BookStack__TokenId=your-token-id \
  -e BookStack__TokenSecret=your-token-secret \
  -e Throttling__Enabled=true \
  -e Throttling__PermitLimit=100 \
  -e Throttling__WindowSeconds=60 \
  --name bookstack-mcp-server \
  bookstack-mcp-server
```

**Configuration Options:**
- `Enabled` - Enable or disable rate limiting (default: `true`)
- `PermitLimit` - Maximum number of requests allowed per time window (default: `100`)
- `WindowSeconds` - Time window in seconds for rate limiting (default: `60`)
- `QueueLimit` - Number of requests to queue when limit is reached (default: `0` - no queuing)

**Notes:**
- Rate limiting is applied per IP address
- Health check endpoints (`/health`, `/health/live`, `/health/ready`) are not rate limited
- When the limit is exceeded, the server returns a `429 Too Many Requests` response
- Set `Enabled` to `false` to disable throttling entirely

### Health Check Endpoints

The server provides ASP.NET Core health check endpoints for monitoring:

- `GET /health` - Comprehensive health check with detailed status information
- `GET /health/ready` - Readiness check (includes BookStack API dependency check)
- `GET /health/live` - Liveness check (returns 200 if the application is running)

Check application health:
```bash
# Liveness check (app is running)
curl http://localhost:8080/health/live

# Readiness check (app is ready to serve requests, including BookStack API)
curl http://localhost:8080/health/ready

# Detailed health information
curl http://localhost:8080/health
```

## BookStack API Setup

1. In your BookStack instance, go to Settings → Users → API Tokens
2. Create a new API token with the required permissions
3. Use the Token ID and Token Secret in your configuration

## Dependencies

- **ASP.NET Core 9** - Web framework
- **ModelContextProtocol.AspNetCore** (0.4.0-preview.1) - MCP server implementation
- **BookStackApiClient** (25.7.0-lib.2) - BookStack API client library
- **Microsoft.Extensions.Diagnostics.HealthChecks** (9.0.9) - Health checks support

## Development Status

- [x] ASP.NET Core 9 project structure
- [x] BookStack API integration
- [x] MCP tool definitions for all major BookStack entities
- [x] HTTP endpoints for testing
- [x] Health checks with BookStack API dependency check
- [x] MCP protocol implementation using official C# SDK
- [x] Optional authentication with HTTP headers
- [x] HTTP throttling/rate limiting
- [x] Unit tests
- [x] Error handling improvements

## License

MIT License - see LICENSE file for details.
