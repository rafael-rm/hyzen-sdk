using Hyzen.SDK.Authentication.DTO;

namespace Hyzen.SDK.Authentication.Services;

public interface IAuthService
{
    public Task<AuthSubject> Verify(string token);
    public Task<string> Login(string email, string password);
}