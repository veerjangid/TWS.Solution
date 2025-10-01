using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TWS.Core.Interfaces.IServices;
using TWS.Core.Settings;

namespace TWS.Infra.Services.Security
{
    /// <summary>
    /// Implementation of encryption service using AES-256 encryption
    /// Follows SecurityDesign.md requirements for field-level encryption
    /// </summary>
    public class EncryptionService : IEncryptionService
    {
        private readonly EncryptionSettings _encryptionSettings;
        private readonly IKeyVaultService _keyVaultService;
        private readonly ILogger<EncryptionService> _logger;
        private string? _encryptionKey;

        public EncryptionService(
            IOptions<EncryptionSettings> encryptionSettings,
            IKeyVaultService keyVaultService,
            ILogger<EncryptionService> logger)
        {
            _encryptionSettings = encryptionSettings.Value;
            _keyVaultService = keyVaultService;
            _logger = logger;
        }

        /// <summary>
        /// Gets the encryption key from Key Vault or configuration
        /// </summary>
        private async Task<string> GetEncryptionKeyAsync()
        {
            // Return cached key if available
            if (!string.IsNullOrEmpty(_encryptionKey))
            {
                return _encryptionKey;
            }

            // Get key from Key Vault if enabled
            if (_encryptionSettings.UseKeyVault)
            {
                try
                {
                    _encryptionKey = await _keyVaultService.GetSecretAsync(_encryptionSettings.KeyVaultSecretName);
                    _logger.LogInformation("Encryption key retrieved from Key Vault");
                    return _encryptionKey;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to retrieve encryption key from Key Vault, falling back to configuration");
                }
            }

            // Fall back to configuration key
            if (string.IsNullOrEmpty(_encryptionSettings.EncryptionKey))
            {
                throw new InvalidOperationException("Encryption key is not configured");
            }

            if (_encryptionSettings.EncryptionKey.Length < 32)
            {
                throw new InvalidOperationException("Encryption key must be at least 32 characters for AES-256");
            }

            _encryptionKey = _encryptionSettings.EncryptionKey;
            _logger.LogInformation("Encryption key retrieved from configuration");
            return _encryptionKey;
        }

        /// <summary>
        /// Encrypts plain text using AES-256 encryption
        /// Returns Base64-encoded string with IV prepended
        /// </summary>
        public async Task<string> EncryptAsync(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                throw new ArgumentException("Plain text cannot be null or empty", nameof(plainText));
            }

            try
            {
                var key = await GetEncryptionKeyAsync();

                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = 256; // AES-256
                    aes.Key = Encoding.UTF8.GetBytes(key.Substring(0, 32)); // Use first 32 characters
                    aes.GenerateIV(); // Generate random IV for each encryption

                    using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        // Prepend IV to the encrypted data
                        msEncrypt.Write(aes.IV, 0, aes.IV.Length);

                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }

                        // Return Base64-encoded encrypted data with IV
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error encrypting data");
                throw;
            }
        }

        /// <summary>
        /// Decrypts encrypted text using AES-256 decryption
        /// Expects Base64-encoded string with IV prepended
        /// </summary>
        public async Task<string> DecryptAsync(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                throw new ArgumentException("Encrypted text cannot be null or empty", nameof(encryptedText));
            }

            try
            {
                var key = await GetEncryptionKeyAsync();
                byte[] fullCipher = Convert.FromBase64String(encryptedText);

                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = 256; // AES-256
                    aes.Key = Encoding.UTF8.GetBytes(key.Substring(0, 32)); // Use first 32 characters

                    // Extract IV from the beginning of the encrypted data
                    byte[] iv = new byte[aes.BlockSize / 8];
                    byte[] cipher = new byte[fullCipher.Length - iv.Length];

                    Array.Copy(fullCipher, iv, iv.Length);
                    Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

                    aes.IV = iv;

                    using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (MemoryStream msDecrypt = new MemoryStream(cipher))
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error decrypting data");
                throw;
            }
        }

        /// <summary>
        /// Masks SSN to show only last 4 digits
        /// Example: 123-45-6789 becomes ***-**-6789
        /// </summary>
        public string MaskSSN(string ssn)
        {
            if (string.IsNullOrEmpty(ssn))
            {
                return string.Empty;
            }

            // Remove any non-digit characters
            string digitsOnly = new string(ssn.Where(char.IsDigit).ToArray());

            if (digitsOnly.Length < 4)
            {
                return "***-**-****";
            }

            // Get last 4 digits
            string lastFour = digitsOnly.Substring(digitsOnly.Length - 4);

            // Return masked format
            return $"***-**-{lastFour}";
        }
    }
}