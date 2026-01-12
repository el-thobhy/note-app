using Administrator.ViewModel;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Administrator.Services
{
    public class AuthServices
    {
        private static readonly HttpClient client = new HttpClient();
        private string routeApi;
        private LoginResponseViewModel response = new LoginResponseViewModel();

        public AuthServices(string apiUrl)
        {
            routeApi = apiUrl;
        }

        public async Task<LoginResponseViewModel> LoginAsync(LoginRequestViewModel model)
        {
            string json = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = await client.PostAsync($"{routeApi}/api/Account/Login", content);

            if (request.IsSuccessStatusCode)
            {
                var apiResponse = await request.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<LoginResponseViewModel>(apiResponse);
            }
            else
            {
                var apiResponse = await request.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<LoginResponseViewModel>(apiResponse);
            }

            return response ?? new LoginResponseViewModel();
        }
        public async Task<ApiResponse> RegisterAccountAsync(RegisterViewModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{routeApi}/api/Account/Registration", content);
            var responseText = await response.Content.ReadAsStringAsync();

            return new ApiResponse
            {
                IsSuccess = response.IsSuccessStatusCode,
                Message = responseText
            };
        }
        public async Task<ApiResponse> VerifyOtpAsync(OtpViewModel otp)
        {

            var content = new StringContent(JsonConvert.SerializeObject(otp), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{routeApi}/api/Account/OtpVerification", content);

            var resultString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new ApiResponse (){ IsSuccess = true, Message=resultString };

            return new ApiResponse() { IsSuccess = false, Message = resultString };
        }
        public async Task<bool> SendOtpAsync(string email)
        {
            ResendOtpViewModel model = new() { 
                Email = email
            };
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Jangan kirim Bearer token karena endpoint ini tidak memerlukannya
            client.DefaultRequestHeaders.Authorization = null;

            var response = await client.PostAsync($"{routeApi}/api/Account/SendOtp", content);

            return response.IsSuccessStatusCode;
        }



    }
}
