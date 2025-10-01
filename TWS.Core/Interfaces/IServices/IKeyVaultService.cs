namespace TWS.Core.Interfaces.IServices
{
    /// <summary>
    /// Service for managing secrets in Azure Key Vault
    /// </summary>
    public interface IKeyVaultService
    {
        /// <summary>
        /// Retrieves a secret value from Azure Key Vault
        /// </summary>
        /// <param name="secretName">Name of the secret to retrieve</param>
        /// <returns>Secret value</returns>
        Task<string> GetSecretAsync(string secretName);

        /// <summary>
        /// Sets or updates a secret value in Azure Key Vault
        /// </summary>
        /// <param name="secretName">Name of the secret to set</param>
        /// <param name="secretValue">Value of the secret</param>
        Task SetSecretAsync(string secretName, string secretValue);
    }
}