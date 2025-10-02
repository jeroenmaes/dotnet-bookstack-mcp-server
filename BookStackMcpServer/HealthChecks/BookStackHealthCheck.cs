using BookStackApi;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BookStackMcpServer.HealthChecks;

public class BookStackHealthCheck : IHealthCheck
{
    private readonly ApiService _apiService;
    private readonly ILogger<BookStackHealthCheck> _logger;

    public BookStackHealthCheck(ApiService apiService, ILogger<BookStackHealthCheck> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Call the BookStack status API endpoint
            // The status endpoint is at /api/status and doesn't require authentication
            var httpClient = new HttpClient();
            var baseUrl = GetBaseUrl();
            var statusUrl = $"{baseUrl}/api/status";
            
            var response = await httpClient.GetAsync(statusUrl, cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogInformation("BookStack status check succeeded: {Content}", content);
                
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

    private string GetBaseUrl()
    {
        // Use reflection to get the base URL from the ApiService
        var field = typeof(ApiService).GetField("_baseUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
        {
            var baseUrl = field.GetValue(_apiService) as string;
            if (!string.IsNullOrEmpty(baseUrl))
            {
                return baseUrl.TrimEnd('/');
            }
        }
        
        throw new InvalidOperationException("Unable to retrieve base URL from ApiService");
    }
}
