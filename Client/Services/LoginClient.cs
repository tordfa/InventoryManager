using BlazorApp1.Shared.Models;
namespace BlazorApp1.Client.Services
{
    public class LoginClient : ILoginClient
    {
        private readonly HttpClient _httpClient;

        public LoginClient(HttpClient httpclient)
        {
            _httpClient = httpclient;
        }
        public Task Login(LoginRequest loginRequest)
        {
            throw new NotImplementedException();
        }
        public void Logout()
        {
            throw new NotSupportedException();
        }

        public Task RefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}
