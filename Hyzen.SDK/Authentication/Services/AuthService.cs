using System.Text;
using Hyzen.SDK.Authentication.DTO;
using Hyzen.SDK.Exception;
using Newtonsoft.Json;

namespace Hyzen.SDK.Authentication.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _client = new();
    public string BaseAddress { get; }
    
    public AuthService()
    {
        BaseAddress = "https://hyzen-auth.azurewebsites.net";
        _client.BaseAddress = new Uri(BaseAddress);
    }

    public async Task<AuthSubject> Verify(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new HException("Invalid or expired token", ExceptionType.InvalidCredentials);
        
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Authorization", token);
        
        var response = await _client.PostAsync("/api/v1/Auth/Verify", null);
        
        if (!response.IsSuccessStatusCode)
            throw new HException("Invalid or expired token", ExceptionType.InvalidCredentials);
        
        var subject = await response.Content.ReadAsStringAsync();
        
        return JsonConvert.DeserializeObject<AuthSubject>(subject);
    }

    public async Task<LoginResponse> Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            throw new HException("Invalid email or password", ExceptionType.InvalidCredentials);
        
        var content = new StringContent(JsonConvert.SerializeObject(new { email, password }), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/v1/Auth/Login", content);

        return !response.IsSuccessStatusCode ? null : JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());
    }

    public async Task<bool> SendRecoveryEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new HException("Invalid email", ExceptionType.InvalidCredentials);
    
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("email", email)
        });

        var response = await _client.PostAsync("/api/v1/Auth/SendRecoveryEmail", content);

        return response.IsSuccessStatusCode;
    }


    public async Task<bool> RecoverPassword(string email, string code, string newPassword)
    {
        var content = new StringContent(JsonConvert.SerializeObject(new { Email = email, VerificationCode = code, NewPassword = newPassword }), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/v1/Auth/RecoverPassword", content);

        return response.IsSuccessStatusCode;
    }
}