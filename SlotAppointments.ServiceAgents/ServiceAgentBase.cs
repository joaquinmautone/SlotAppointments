namespace SlotAppointments.ServiceAgents
{
    using Microsoft.Extensions.Options;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using SlotAppointments.ServiceAgents.Availability.Configuration;
    using System.Text.Json;

    public abstract class ServiceAgentBase
    {
        protected readonly HttpClient _httpClient;
        private readonly string _username;
        private readonly string _password;
        
        protected ServiceAgentBase(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(apiSettings.Value.BaseUrl);            
            _username = apiSettings.Value.Username;
            _password = apiSettings.Value.Password;

            AddBasicAuthentication();
        }

        private void AddBasicAuthentication()
        {
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_username}:{_password}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        }

        protected async Task<T> GetAsync<T>(string requestUrl, JsonSerializerOptions? options = null)
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}{requestUrl}");
            
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(jsonResponse, options);
            }

            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }

        protected async Task<bool> PostAsync(string requestUrl, object requestBody)
        {
            string jsonData = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}{requestUrl}", content);
            response.EnsureSuccessStatusCode();
            return true;
        }
    }

}
