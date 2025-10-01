namespace TWS.Core.Settings
{
    /// <summary>
    /// Configuration settings for Azure services integration
    /// </summary>
    public class AzureSettings
    {
        /// <summary>
        /// Connection string for Azure Blob Storage
        /// For local development, use: "UseDevelopmentStorage=true"
        /// </summary>
        public string BlobStorageConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Default container name for storing documents
        /// </summary>
        public string BlobStorageContainerName { get; set; } = "tws-documents";

        /// <summary>
        /// Azure Key Vault URL for secret management
        /// Format: https://your-keyvault.vault.azure.net/
        /// </summary>
        public string KeyVaultUrl { get; set; } = string.Empty;

        /// <summary>
        /// Toggle to enable/disable Azure Blob Storage
        /// When false, falls back to local file storage
        /// </summary>
        public bool UseBlobStorage { get; set; } = false;
    }
}