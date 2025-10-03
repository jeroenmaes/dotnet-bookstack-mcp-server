using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using BookStackMcpServer.Models;

namespace BookStackMcpServer.HealthChecks;

public class BookStackHealthCheck : IHealthCheck
{
    private readonly string _baseUrl;
    private readonly ILogger<BookStackHealthCheck> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public BookStackHealthCheck(IOptions<BookStackOptions> options, ILogger<BookStackHealthCheck> logger, IHttpClientFactory httpClientFactory)
    {
        _baseUrl = options.Value.BaseUrl.TrimEnd('/');
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Call the BookStack status endpoint
            // The status endpoint is at /status and doesn't require authentication
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(5);
            
            var statusUrl = $"{_baseUrl}/status";
            
            var response = await httpClient.GetAsync(statusUrl, cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogInformation("BookStack status check succeeded");
                
                return HealthCheckResult.Healthy("BookStack API is responding", new Dictionary<string, object>
                {
                    { "statusUrl", statusUrl },
                    { "response", content }
                });
            }
            else
            {
                _logger.LogWarning("BookStack status check returned status code: {StatusCode}", response.StatusCode);
                
                return HealthCheckResult.Degraded($"BookStack API returned status code: {response.StatusCode}", null, new Dictionary<string, object>
                {
                    { "statusUrl", statusUrl },
                    { "statusCode", (int)response.StatusCode }
                });
            }
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogError(ex, "BookStack status check timed out after 5 seconds");
            
            return HealthCheckResult.Unhealthy("BookStack API health check timed out", ex, new Dictionary<string, object>
            {
                { "error", "Request timed out after 5 seconds" }
            });
        }
        catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning(ex, "BookStack status check was cancelled");
            
            return HealthCheckResult.Unhealthy("BookStack API health check was cancelled", ex, new Dictionary<string, object>
            {
                { "error", "Health check was cancelled" }
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "BookStack status check failed with HTTP request exception");
            
            return HealthCheckResult.Unhealthy("BookStack API is not reachable", ex, new Dictionary<string, object>
            {
                { "error", ex.Message }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "BookStack status check failed with unexpected exception");
            
            return HealthCheckResult.Unhealthy("BookStack API health check failed", ex, new Dictionary<string, object>
            {
                { "error", ex.Message }
            });
        }
    }
}
