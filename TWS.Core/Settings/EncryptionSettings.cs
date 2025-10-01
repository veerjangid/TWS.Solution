namespace TWS.Core.Settings
{
    /// <summary>
    /// Configuration settings for data encryption
    /// </summary>
    public class EncryptionSettings
    {
        /// <summary>
        /// AES-256 encryption key for field-level encryption
        /// Must be at least 32 characters for AES-256
        /// In production, this should be stored in Azure Key Vault
        /// </summary>
        public string EncryptionKey { get; set; } = string.Empty;

        /// <summary>
        /// Toggle to use Azure Key Vault for encryption key management
        /// When true, retrieves encryption key from Key Vault
        /// When false, uses EncryptionKey from appsettings
        /// </summary>
        public bool UseKeyVault { get; set; } = false;

        /// <summary>
        /// Name of the secret in Azure Key Vault containing the encryption key
        /// </summary>
        public string KeyVaultSecretName { get; set; } = "EncryptionKey";
    }
}