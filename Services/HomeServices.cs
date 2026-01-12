using System.Net.Http.Headers;
using System.Text;
using Administrator.ViewModel;
using Newtonsoft.Json;

namespace Administrator.Services
{
    public interface IAccountService
    {
        Task<List<AccountViewModel>> GetAccountsAsync();
        Task<bool> UpdateUserRoleAsync(UpdateUserRoleRequest request, string adminUserName);
        Task<bool> DeleteAccountAsync(DeleteAccountRequest request, string adminUserName);

    }
    public class AccountService : IAccountService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _routeApi;

        public AccountService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _client = httpClientFactory.CreateClient();
            _httpContextAccessor = httpContextAccessor;
            _routeApi = config["ApiUrl"]; // langsung ambil dari config DI SINI
        }

        public async Task<bool> DeleteAccountAsync(DeleteAccountRequest request, string adminUserName)
        {

            string token = _httpContextAccessor?.HttpContext?.Session.GetString("Token") ?? "";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var requestBody = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_routeApi}/api/Admin/DeleteAccount?admin={adminUserName}"),
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(requestBody);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<AccountViewModel>> GetAccountsAsync()
        {
            string token = _httpContextAccessor?.HttpContext?.Session.GetString("Token") ?? ""; 
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{_routeApi}/api/Admin/GetListAccount");

            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AccountApiResponse>(resultString);
                return result?.data ?? [];
            }

            return new List<AccountViewModel>();
        }

        public async Task<bool> UpdateUserRoleAsync(UpdateUserRoleRequest request, string adminUserName)
        {
            string token = _httpContextAccessor?.HttpContext?.Session.GetString("Token") ?? "";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{_routeApi}/api/Admin/UpdateUserRole?admin={adminUserName}", content);
            return response.IsSuccessStatusCode;
        }

    }

}
