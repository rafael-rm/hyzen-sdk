using System.Net.Http.Headers;
using Hyzen.SDK.Authentication.DTO;
using Hyzen.SDK.Exception;
using Newtonsoft.Json;

namespace Hyzen.SDK.Authentication;

public static class AuthService
{
    private const string Url = "http://localhost:5209";
    
    public static async Task<AuthSubject> Verify(string token)
    {
        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("Authorization", token);
        
        client.BaseAddress = new Uri(Url);
        
        var response = await client.PostAsync("/api/v1/Auth/Verify", null);
        
        if (!response.IsSuccessStatusCode)
            throw new HException("[Hyzen Auth] Invalid token", ExceptionType.InvalidCredentials);
        
        var subject = await response.Content.ReadAsStringAsync();
        
        return JsonConvert.DeserializeObject<AuthSubject>(subject);
    }
}