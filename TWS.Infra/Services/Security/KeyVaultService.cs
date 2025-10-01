using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TWS.Core.Interfaces.IServices;
using TWS.Core.Settings;

namespace TWS.Infra.Services.Security
{
    /// <summary>
    /// Implementation of Azure Key Vault service for secret management
    /// </summary>
    public class KeyVaultService : IKeyVaultService
    {
        private readonly SecretClient? _secretClient;
        private readonly AzureSettings _azureSettings;
        private readonly ILogger<KeyVaultService> _logger;
        private readonly Dictionary<string, string> _secretCache;

        public KeyVaultService(
            IOptions<AzureSettings> azureSettings,
            ILogger<KeyVaultService> logger)
        {
            _azureSettings = azureSettings.Value;
            _logger = logger;
            _secretCache = new Dictionary<string, string>();

            // Initialize SecretClient with Key Vault URL
            if (!string.IsNullOrEmpty(_azureSettings.KeyVaultUrl))
            {
                try
                {
                    // Use DefaultAzureCredential for authentication
                    // This supports multiple authentication methods:
                    // - Environment variables
                    // - Managed Identity
                    // - Visual Studio
                    // - Azure CLI
                    // - Interactive browser
                    _secretClient = new SecretClient(
                        new Uri(_azureSettings.KeyVaultUrl),
                        new DefaultAzureCredential());

                    _logger.LogInformation("Key Vault client initialized successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to initialize Key Vault client");
                }
            }
            else
            {
                _logger.LogWarning("Azure Key Vault URL is not configured");
            }
        }

        /// <summary>
        /// Retrieves a secret value from Azure Key Vault with caching
        /// </summary>
        public async Task<string> GetSecretAsync(string secretName)
        {
            try
            {
                // Check cache first
                if (_secretCache.TryGetValue(secretName, out var cachedValue))
                {
                    _logger.LogDebug("Retrieved secret {SecretName} from cache", secretName);
                    return cachedValue;
                }

                if (_secretClient == null)
                {
                    throw new InvalidOperationException("Key Vault is not configured properly");
                }

                // Retrieve from Key Vault
                KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
                var secretValue = secret.Value;

                // Cache the secret
                _secretCache[secretName] = secretValue;

                _logger.LogInformation("Successfully retrieved secret {SecretName} from Key Vault", secretName);

                return secretValue;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving secret {SecretName} from Key Vault", secretName);
                throw;
            }
        }

        /// <summary>
        /// Sets or updates a secret value in Azure Key Vault
        /// </summary>
        public async Task SetSecretAsync(string secretName, string secretValue)
        {
            try
            {
                if (_secretClient == null)
                {
                    throw new InvalidOperationException("Key Vault is not configured properly");
                }

                // Set or update secret in Key Vault
                await _secretClient.SetSecretAsync(secretName, secretValue);

                // Update cache
                _secretCache[secretName] = secretValue;

                _logger.LogInformation("Successfully set secret {SecretName} in Key Vault", secretName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting secret {SecretName} in Key Vault", secretName);
                throw;
            }
        }
    }
}