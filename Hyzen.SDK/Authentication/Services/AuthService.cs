using System.Text;
using Hyzen.SDK.Authentication.DTO;
using Hyzen.SDK.Exception;
using Newtonsoft.Json;

namespace Hyzen.SDK.Authentication.Services;

public class AuthService : IAuthService
{
    private const string Url = "https://hyzen-auth.azurewebsites.net";
    
    public async Task<AuthSubject> Verify(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new HException("Invalid or expired token", ExceptionType.InvalidCredentials);
        
        using HttpClient client = new(); // TODO: Fix socket exhaustion
        client.DefaultRequestHeaders.Add("Authorization", token);
        
        client.BaseAddress = new Uri(Url);
        
        var response = await client.PostAsync("/api/v1/Auth/Verify", null);
        
        if (!response.IsSuccessStatusCode)
            throw new HException("Invalid or expired token", ExceptionType.InvalidCredentials);
        
        var subject = await response.Content.ReadAsStringAsync();
        
        return JsonConvert.DeserializeObject<AuthSubject>(subject);
    }

    public async Task<LoginResponse> Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            throw new HException("Invalid email or password", ExceptionType.InvalidCredentials);
        
        using HttpClient client = new(); // TODO: Fix socket exhaustion
        client.BaseAddress = new Uri(Url);
        
        var content = new StringContent(JsonConvert.SerializeObject(new { email, password }), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/v1/Auth/Login", content);
        
        if (!response.IsSuccessStatusCode)
            throw new HException("Invalid email or password", ExceptionType.InvalidCredentials);
        
        return JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());
    }
}