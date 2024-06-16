using System.Net.Http.Headers;
using Hyzen.SDK.Authentication.DTO;
using Hyzen.SDK.Exception;
using Newtonsoft.Json;

namespace Hyzen.SDK.Authentication;

public static class AuthService
{
    private const string Url = "http://localhost:5209";
    
    public static AuthSubject Verify(string token)
    {
        using HttpClient client = new();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        client.BaseAddress = new Uri(Url);
        
        var response = client.PostAsync("/api/v1/Auth/Verify", null).Result;
        
        if (!response.IsSuccessStatusCode)
            throw new HException("[Hyzen Auth] Invalid token", ExceptionType.InvalidCredentials);
        
        var subject = response.Content.ReadAsStringAsync().Result;
        
        return JsonConvert.DeserializeObject<AuthSubject>(subject);
    }
}