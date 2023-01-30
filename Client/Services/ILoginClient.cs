using BlazorApp1.Shared.Models;
namespace BlazorApp1.Client.Services
{
    public interface ILoginClient
    {
        Task Login(LoginRequest loginRequest);
        void Logout();

        Task RefreshToken();
    }
}