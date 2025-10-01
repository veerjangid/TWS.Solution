using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace TWS.Web.Services.HttpServices;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiClient> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        // Try to get token from session on initialization
        var token = GetTokenFromSession();
        if (!string.IsNullOrEmpty(token))
        {
            SetAuthToken(token);
        }
    }

    public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
    {
        try
        {
            _logger.LogInformation("API GET Request to: {Endpoint}", endpoint);

            var response = await _httpClient.GetAsync(endpoint);
            return await ProcessResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during GET request to {Endpoint}", endpoint);
            return CreateErrorResponse<T>(ex.Message);
        }
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data)
    {
        try
        {
            _logger.LogInformation("API POST Request to: {Endpoint}", endpoint);

            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            return await ProcessResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during POST request to {Endpoint}", endpoint);
            return CreateErrorResponse<T>(ex.Message);
        }
    }

    public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object data)
    {
        try
        {
            _logger.LogInformation("API PUT Request to: {Endpoint}", endpoint);

            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(endpoint, content);
            return await ProcessResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during PUT request to {Endpoint}", endpoint);
            return CreateErrorResponse<T>(ex.Message);
        }
    }

    public async Task<ApiResponse<T>> DeleteAsync<T>(string endpoint)
    {
        try
        {
            _logger.LogInformation("API DELETE Request to: {Endpoint}", endpoint);

            var response = await _httpClient.DeleteAsync(endpoint);
            return await ProcessResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during DELETE request to {Endpoint}", endpoint);
            return CreateErrorResponse<T>(ex.Message);
        }
    }

    public void SetAuthToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        _logger.LogInformation("Auth token set for API client");
    }

    public void ClearAuthToken()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
        _logger.LogInformation("Auth token cleared from API client");
    }

    private async Task<ApiResponse<T>> ProcessResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        _logger.LogInformation("API Response Status: {StatusCode}", response.StatusCode);

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(content, _jsonOptions);

                if (apiResponse != null)
                {
                    apiResponse.StatusCode = (int)response.StatusCode;
                    return apiResponse;
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error deserializing API response");
            }
        }

        // Handle error responses
        try
        {
            var errorResponse = JsonSerializer.Deserialize<ApiResponse<T>>(content, _jsonOptions);
            if (errorResponse != null)
            {
                errorResponse.StatusCode = (int)response.StatusCode;
                return errorResponse;
            }
        }
        catch (JsonException)
        {
            // If we can't deserialize the error, return a generic error response
        }

        return new ApiResponse<T>
        {
            Success = false,
            Message = $"API request failed with status code: {response.StatusCode}",
            StatusCode = (int)response.StatusCode,
            Errors = new List<string> { content }
        };
    }

    private ApiResponse<T> CreateErrorResponse<T>(string errorMessage)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = errorMessage,
            StatusCode = 500,
            Errors = new List<string> { errorMessage }
        };
    }

    private string? GetTokenFromSession()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            return httpContext.Session.GetString("JwtToken");
        }
        return null;
    }
}