namespace TWS.Core.Interfaces.IServices
{
    /// <summary>
    /// Service for managing file storage in Azure Blob Storage
    /// </summary>
    public interface IBlobStorageService
    {
        /// <summary>
        /// Uploads a file to Azure Blob Storage
        /// </summary>
        /// <param name="containerName">Container name where file will be stored</param>
        /// <param name="fileName">Name of the file (will be made unique)</param>
        /// <param name="fileStream">File stream to upload</param>
        /// <param name="contentType">MIME type of the file</param>
        /// <returns>URL of the uploaded blob</returns>
        Task<string> UploadFileAsync(string containerName, string fileName, Stream fileStream, string contentType);

        /// <summary>
        /// Downloads a file from Azure Blob Storage
        /// </summary>
        /// <param name="containerName">Container name where file is stored</param>
        /// <param name="fileName">Name of the file to download</param>
        /// <returns>Stream containing the file data</returns>
        Task<Stream> DownloadFileAsync(string containerName, string fileName);

        /// <summary>
        /// Deletes a file from Azure Blob Storage
        /// </summary>
        /// <param name="containerName">Container name where file is stored</param>
        /// <param name="fileName">Name of the file to delete</param>
        /// <returns>True if deleted successfully, false otherwise</returns>
        Task<bool> DeleteFileAsync(string containerName, string fileName);

        /// <summary>
        /// Checks if a file exists in Azure Blob Storage
        /// </summary>
        /// <param name="containerName">Container name where file might be stored</param>
        /// <param name="fileName">Name of the file to check</param>
        /// <returns>True if file exists, false otherwise</returns>
        Task<bool> FileExistsAsync(string containerName, string fileName);
    }
}