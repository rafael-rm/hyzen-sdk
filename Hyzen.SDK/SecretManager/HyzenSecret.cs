using Hyzen.SDK.SecretManager.Services;

namespace Hyzen.SDK.SecretManager;

public static class HyzenSecret
{
    public static ISecretService Service { get; set; }
    
    static HyzenSecret()
    {
        Service = new AzureVaultService();
    }

    public static async Task<string> GetSecretAsync(string secretName)
    {
        return await Service.GetSecret(secretName);
    }

    public static async Task<string> GetSecretAsync(string secretName, string version)
    {
        return await Service.GetSecret(secretName, version);
    }
    
    public static string GetSecret(string secretName)
    {
        return GetSecretAsync(secretName).GetAwaiter().GetResult();
    }
    
    public static string GetSecret(string secretName, string version)
    {
        return GetSecretAsync(secretName, version).GetAwaiter().GetResult();
    }
}