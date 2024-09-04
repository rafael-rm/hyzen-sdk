namespace Hyzen.SDK.Authentication.DTO;

public class LoginResponse
{
    public AuthSubject Subject { get; set; }
    public string Token { get; set; }
}