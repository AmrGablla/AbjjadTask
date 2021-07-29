using WepAPI.Models;

namespace WepAPI.Services
{
    public interface IAuthService
    {
        AuthenticationResponse AuthenticateAsync(AuthenticationRequest request);
        bool RegisterAsync(RegisterRequest request);
    }
}