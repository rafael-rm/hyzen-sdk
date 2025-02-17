﻿using Hyzen.SDK.Authentication.DTO;
using Hyzen.SDK.Authentication.Services;
using Hyzen.SDK.Exception;

namespace Hyzen.SDK.Authentication;

public static class HyzenAuth
{
    private static readonly AsyncLocal<string> Token = new();
    private static readonly AsyncLocal<AuthSubject> Subject = new();
    private static readonly IAuthService Service;

    static HyzenAuth()
    {
        Service = new AuthService();
    }
    
    public static string GetToken()
    {
        return Token.Value;
    }
    
    public static void SetToken(string token)
    {
        Token.Value = token;
    }
    
    private static async Task<AuthSubject> Verify()
    {
        return await Service.Verify(GetToken());
    }
    
    public static async Task<AuthSubject> GetSubject()
    {
        if (Subject.Value == null)
            Subject.Value = await Verify();

        return Subject.Value;
    }
    
    public static async Task<LoginResponse> Login(string email, string password)
    {
        return await Service.Login(email, password);
    }
    
    public static async Task<bool> SendRecoveryEmail(string email)
    {
        return await Service.SendRecoveryEmail(email);
    }
    
    public static async Task<bool> RecoverPassword(string email, string code, string newPassword)
    {
        return await Service.RecoverPassword(email, code, newPassword);
    }
    
    public static void SetSubject(AuthSubject subject)
    {
        Subject.Value = subject;
    }
    
    public static async Task EnsureAuthenticated()
    {
        if (string.IsNullOrWhiteSpace(GetToken()))
            throw new HException("Invalid or expired token", ExceptionType.InvalidCredentials);
        
        await GetSubject();
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
            throw new HException($"Role '{roleKey}' is required for this action", ExceptionType.PermissionRequired);
    }
    
    public static async Task EnsureGroup(string groupKey)
    {
        if (!await HasGroup(groupKey))
            throw new HException($"Group '{groupKey}' is required for this action", ExceptionType.PermissionRequired);
    }
    
    public static async Task<bool> IsAdmin()
    {
        return await HasGroup("Admin");
    }
    
    public static Task EnsureAdmin()
    {
        return EnsureGroup("Admin");
    }
    
    public static string GetBaseAddress()
    {
        return Service.BaseAddress;
    }
}