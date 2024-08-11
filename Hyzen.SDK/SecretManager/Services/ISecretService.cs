namespace Hyzen.SDK.SecretManager.Services;

public interface ISecretService
{
    public Task<string> GetSecret(string secretName);
    public Task<string> GetSecret(string secretName, string version);
    
}