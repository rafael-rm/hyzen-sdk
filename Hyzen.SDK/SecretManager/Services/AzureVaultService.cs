using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Hyzen.SDK.SecretManager.Services;

public class AzureVaultService : ISecretService
{
    private SecretClientOptions Options { get; set; }
    private SecretClient Client { get; set; }
    
    public AzureVaultService()
    {
        Options = new SecretClientOptions()
        {
            Retry =
            {
                Delay= TimeSpan.FromSeconds(2),
                MaxDelay = TimeSpan.FromSeconds(16),
                MaxRetries = 5,
                Mode = RetryMode.Exponential
            }
        };
        
        Client = new SecretClient(new Uri("https://hyzen-vault-generic.vault.azure.net/"), new DefaultAzureCredential(), Options);
    }
    
    public async Task<string> GetSecret(string secretName)
    {
        var secret = await Client.GetSecretAsync(secretName);
        return secret.HasValue ? secret.Value.Value : null;
    }

    public async Task<string> GetSecret(string secretName, string version)
    {
        var secret = await Client.GetSecretAsync(secretName, version);
        return secret.Value.Value;
    }
}