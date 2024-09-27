using Hyzen.SDK.Authentication.DTO;

namespace Hyzen.SDK.Authentication.Services;

public interface IAuthService
{
    public Task<AuthSubject> Verify(string token);
    public Task<LoginResponse> Login(string email, string password);
    public Task<bool> SendRecoveryEmail(string email);
    public Task<bool> RecoverPassword(string email, string code, string newPassword);
}