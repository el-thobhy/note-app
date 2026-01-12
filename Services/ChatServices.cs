namespace Administrator.Services
{
    using Administrator.ViewModel;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using System.Net.Http.Headers;
    using System.Text;

    namespace auth_project.Services
    {
        public interface IChatService
        {
            Task<List<MessageViewModel>> GetChatHistoryAsync(string userId1, string userId2);
        }
        public class ChatService : IChatService
        {
            private readonly HttpClient _client;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly string _routeApi;

            public ChatService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration config)
            {
                _client = httpClientFactory.CreateClient();
                _httpContextAccessor = httpContextAccessor;
                _routeApi = config["ApiUrl"];
            }

            public async Task<List<MessageViewModel>> GetChatHistoryAsync(string userId1, string userId2)
            {
                string token = _httpContextAccessor?.HttpContext?.Session?.GetString("Token") ?? "";
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"{_routeApi}/api/Chat/history?userId1={userId1}&userId2={userId2}";
                var response = await _client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<MessageViewModel>>(json);
                    return result ?? new List<MessageViewModel>();
                }

                return new List<MessageViewModel>();
            }
        }
    }

}
