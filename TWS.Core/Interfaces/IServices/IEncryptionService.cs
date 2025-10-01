namespace TWS.Core.Interfaces.IServices
{
    /// <summary>
    /// Service for encrypting and decrypting sensitive data using AES-256
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Encrypts plain text using AES-256 encryption
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <returns>Base64-encoded encrypted text</returns>
        Task<string> EncryptAsync(string plainText);

        /// <summary>
        /// Decrypts encrypted text using AES-256 decryption
        /// </summary>
        /// <param name="encryptedText">Base64-encoded encrypted text</param>
        /// <returns>Decrypted plain text</returns>
        Task<string> DecryptAsync(string encryptedText);

        /// <summary>
        /// Masks SSN to show only last 4 digits
        /// </summary>
        /// <param name="ssn">Social Security Number</param>
        /// <returns>Masked SSN in format ***-**-1234</returns>
        string MaskSSN(string ssn);
    }
}