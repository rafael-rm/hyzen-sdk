﻿using Hyzen.SDK.Authentication.DTO;
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
    
    public static async Task<AuthSubject> GetSubject()
    {
        if (Subject.Value == null)
            return await Verify();
        
        return Subject.Value;
    }
    
    public static async Task<AuthSubject> Verify(string token)
    {
        return await AuthService.Verify(token);
    }
    
    public static async Task<AuthSubject> Verify()
    {
        return await AuthService.Verify(GetToken());
    }
    
    public static async Task<bool> HasRole(string roleKey)
    {
        return (await GetSubject()).HasRole(roleKey);
    }
    
    public static async Task<bool> HasGroup(string groupKey)
    {
        return (await GetSubject()).HasGroup(groupKey);
    }
    
    public static async Task EnsureRole(string roleKey)
    {
        if (!await HasRole(roleKey))
            throw new HException($"[Hyzen Auth] Role '{roleKey}' is required", ExceptionType.PermissionRequired);
    }
    
    public static async Task EnsureGroup(string groupKey)
    {
        if (!await HasGroup(groupKey))
            throw new HException($"[Hyzen Auth] Group '{groupKey}' is required", ExceptionType.PermissionRequired);
    }
}