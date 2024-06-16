using Hyzen.SDK.Authentication.DTO;
using Hyzen.SDK.Exception;

namespace Hyzen.SDK.Authentication;

public class Auth
{
    private static readonly AsyncLocal<string> Token = new();
    private static readonly AsyncLocal<AuthSubject> Subject = new();

    public static void SetToken(string token) => Token.Value = token;
    
    public static string GetToken()
    {
        return Token.Value;
    }
    
    public static AuthSubject GetSubject()
    {
        if (Subject.Value == null)
            Subject.Value = Verify();
        
        return Subject.Value;
    }
    
    public static AuthSubject Verify(string token)
    {
        return AuthService.Verify(token);
    }
    
    public static AuthSubject Verify()
    {
        return AuthService.Verify(GetToken());
    }
    
    public static bool HasRole(string roleKey)
    {
        return GetSubject().HasRole(roleKey);
    }
    
    public static bool HasGroup(string groupKey)
    {
        return GetSubject().HasGroup(groupKey);
    }
    
    public static void EnsureRole(string roleKey)
    {
        if (!HasRole(roleKey))
            throw new HException($"[Hyzen Auth] Role '{roleKey}' is required", ExceptionType.PermissionRequired);
    }
    
    public static void EnsureGroup(string groupKey)
    {
        if (!HasGroup(groupKey))
            throw new HException($"[Hyzen Auth] Group '{groupKey}' is required", ExceptionType.PermissionRequired);
    }
}