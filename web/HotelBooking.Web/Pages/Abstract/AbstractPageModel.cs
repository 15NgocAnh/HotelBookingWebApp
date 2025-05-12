using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HotelBooking.Web.Pages.Abstract
{
    public abstract class AbstractPageModel : PageModel
    {
        protected readonly IConfiguration _configuration;
        protected readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _jsonOptions;

        public AbstractPageModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("BackendApi");
            _httpClient.BaseAddress = new Uri(_configuration["BackendApi"]);
            _httpContextAccessor = httpContextAccessor;
            
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        public class ApiResponse<T>
        {
            [JsonPropertyName("data")]
            public T Data { get; set; }
        }

        public class ErrorResponse
        {
            public Dictionary<string, List<string>> Errors { get; set; } = new();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Response</typeparam>
        /// <param name="endpoint">Request</param>
        /// <returns></returns>
        public async Task<T?> GetAsync<T>(string endpoint) => await SendRequestAsync<T>(HttpMethod.Get, endpoint);

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
            => await SendRequestAsync<TResponse>(HttpMethod.Post, endpoint, data);

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
            => await SendRequestAsync<TResponse>(HttpMethod.Put, endpoint, data);

        public async Task<TResponse?> DeleteAsync<TResponse>(string endpoint)
            => await SendRequestAsync<TResponse>(HttpMethod.Delete, endpoint);

        private async Task<TResponse?> SendRequestAsync<TResponse>(HttpMethod method, string endpoint, object? data = null)
        {
            var client = CreateClientWithToken();
            if (client == null) return default;

            HttpResponseMessage response;
            if (data != null)
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(data, _jsonOptions), Encoding.UTF8, "application/json");
                response = method.Method switch
                {
                    "POST" => await client.PostAsync(endpoint, jsonContent),
                    "PUT" => await client.PutAsync(endpoint, jsonContent),
                    _ => throw new NotSupportedException("Method not supported")
                };
            }
            else
            {
                response = await client.SendAsync(new HttpRequestMessage(method, endpoint));
            }

            return await ProcessResponse<TResponse>(response);
        }

        private async Task<T?> ProcessResponse<T>(HttpResponseMessage response)
        {
            var responseText = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseText, _jsonOptions);
                    if (errorResponse?.Errors != null)
                    {
                        throw new InvalidOperationException(string.Join(", ", errorResponse.Errors.Values.SelectMany(e => e)));
                    }
                }
                catch
                {
                    // If we can't parse the error response, just throw the raw response
                    throw new InvalidOperationException($"API Error: {response.StatusCode} - {responseText}");
                }
            }

            try
            {
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(responseText, _jsonOptions);
                return apiResponse.Data ?? default;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to parse API response: {ex.Message}");
            }
        }

        private HttpClient? CreateClientWithToken()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["JWT"];
            if (string.IsNullOrEmpty(token))
                return null;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return _httpClient;
        }
    }
}
