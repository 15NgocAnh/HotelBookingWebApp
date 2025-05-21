using HotelBooking.Application.Common.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace HotelBooking.Web.Services
{
    public interface IApiService
    {
        Task<Result<T>?> GetAsync<T>(string endpoint, bool retrying = false);
        Task<Result<T>?> PostAsync<T>(string endpoint, object data, bool retrying = false);
        Task<Result> PutAsync<T>(string endpoint, object data, bool retrying = false);
        Task<Result> DeleteAsync(string endpoint, bool retrying = false);
    }

    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenService _tokenService;
        private readonly ILogger<ApiService> _logger;

        public ApiService(
            IHttpClientFactory httpClientFactory,
            ITokenService tokenService,
            ILogger<ApiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
            _logger = logger;
        }

        private async Task<HttpClient> CreateClientAsync()
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            var token = await _tokenService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        private async Task<bool> HandleUnauthorizedAsync(HttpClient client)
        {
            var ok = await _tokenService.RefreshTokenAsync();
            if (!ok) return false;

            var newToken = await _tokenService.GetTokenAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
            return true;
        }

        public async Task<Result<T>?> GetAsync<T>(string endpoint, bool retrying = false)
        {
            try
            {
                var client = await CreateClientAsync();
                var response = await client.GetAsync(endpoint);
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && !retrying)
                {
                    if (await HandleUnauthorizedAsync(client))
                        return await GetAsync<T>(endpoint, true);

                    return Result<T>.Failure("Unauthorized access");
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("API call to {Endpoint} failed: {Content}", endpoint, content);
                    return HandleErrorResponse<T>(content);
                }

                var resultData = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Result<T>.Success(resultData!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting data from {Endpoint}", endpoint);
                return Result<T>.Failure("Exception occurred while getting data.", ex);
            }
        }

        public async Task<Result<T>?> PostAsync<T>(string endpoint, object data, bool retrying = false)
        {
            try
            {
                var client = await CreateClientAsync();
                var response = await client.PostAsJsonAsync(endpoint, data);
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && !retrying)
                {
                    if (await HandleUnauthorizedAsync(client))
                        return await PostAsync<T>(endpoint, data, true);

                    return Result<T>.Failure("Unauthorized access");
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("API call to {Endpoint} failed: {Content}", endpoint, content);
                    return HandleErrorResponse<T>(content);
                }

                var resultData = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Result<T>.Success(resultData!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error posting data to {Endpoint}", endpoint);
                return Result<T>.Failure("Exception occurred while posting data.", ex);
            }
        }

        public async Task<Result> PutAsync<T>(string endpoint, object data, bool retrying = false)
        {
            try
            {
                var client = await CreateClientAsync();
                var response = await client.PutAsJsonAsync(endpoint, data);
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && !retrying)
                {
                    if (await HandleUnauthorizedAsync(client))
                        return await PutAsync<T>(endpoint, data, true);

                    return Result.Failure("Unauthorized access");
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("API call to {Endpoint} failed: {Content}", endpoint, content);
                    return HandleErrorResponse(content);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error putting data to {Endpoint}", endpoint);
                return Result.Failure("Exception occurred while updating data.", ex);
            }
        }

        public async Task<Result> DeleteAsync(string endpoint, bool retrying = false)
        {
            try
            {
                var client = await CreateClientAsync();
                var response = await client.DeleteAsync(endpoint);
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && !retrying)
                {
                    if (await HandleUnauthorizedAsync(client))
                        return await DeleteAsync(endpoint, true);

                    return Result.Failure("Unauthorized access");
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("API call to {Endpoint} failed: {Content}", endpoint, content);
                    return HandleErrorResponse(content);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting data from {Endpoint}", endpoint);
                return Result.Failure("Exception occurred while deleting data.", ex);
            }
        }

        private Result<T> HandleErrorResponse<T>(string content)
        {
            try
            {
                if (content != null)
                {
                    var doc = JsonDocument.Parse(content);
                    var root = doc.RootElement;

                    if (root.ValueKind == JsonValueKind.Object)
                    {
                        if (root.TryGetProperty("messages", out var messagesEl))
                        {
                            if (messagesEl.ValueKind == JsonValueKind.Array)
                            {
                                var apiError = JsonSerializer.Deserialize<ResultError[]>(messagesEl.GetRawText(), new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                });

                                return new Result<T>
                                {
                                    IsSuccess = false,
                                    Messages = apiError ?? []
                                };
                            }
                        }
                    }
                    else if (root.ValueKind == JsonValueKind.Array)
                    {
                        var apiError = JsonSerializer.Deserialize<ResultError[]>(content, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return new Result<T>
                        {
                            IsSuccess = false,
                            Messages = apiError ?? []
                        };
                    }
                    else if (root.ValueKind == JsonValueKind.String)
                    {
                        return new Result<T>
                        {
                            IsSuccess = false,
                            Messages = [new ResultError { Field = "", Message = root.GetString() ?? "Unknown error" }]
                        };
                    }
                }

                return Result<T>.Failure("API returned an error without details", null);
            }
            catch (Exception jsonEx)
            {
                _logger.LogError(jsonEx, "Failed to deserialize error response");
                return Result<T>.Failure("API error: Unable to parse error message", new Exception(content));
            }
        }

        private Result HandleErrorResponse(string content)
        {
            try
            {
                var doc = JsonDocument.Parse(content);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Object)
                {
                    if (root.TryGetProperty("messages", out var messagesEl))
                    {
                        if (messagesEl.ValueKind == JsonValueKind.Array)
                        {
                            var apiError = JsonSerializer.Deserialize<ResultError[]>(messagesEl.GetRawText(), new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                            return new Result
                            {
                                IsSuccess = false,
                                Messages = apiError ?? []
                            };
                        }
                    }
                }
                else if (root.ValueKind == JsonValueKind.Array)
                {
                    var apiError = JsonSerializer.Deserialize<ResultError[]>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return new Result
                    {
                        IsSuccess = false,
                        Messages = apiError ?? []
                    };
                }
                else if (root.ValueKind == JsonValueKind.String)
                {
                    return new Result
                    {
                        IsSuccess = false,
                        Messages = [new ResultError { Field = "", Message = root.GetString() ?? "Unknown error" }]
                    };
                }

                return Result.Failure("API returned an error without details", null);
            }
            catch (Exception jsonEx)
            {
                _logger.LogError(jsonEx, "Failed to deserialize error response");
                return Result.Failure("API error: Unable to parse error message", new Exception(content));
            }
        }
    }
} 