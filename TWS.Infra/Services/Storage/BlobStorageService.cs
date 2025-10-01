using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TWS.Core.Interfaces.IServices;
using TWS.Core.Settings;

namespace TWS.Infra.Services.Storage
{
    /// <summary>
    /// Implementation of Azure Blob Storage service for file management
    /// </summary>
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient? _blobServiceClient;
        private readonly AzureSettings _azureSettings;
        private readonly ILogger<BlobStorageService> _logger;

        public BlobStorageService(
            IOptions<AzureSettings> azureSettings,
            ILogger<BlobStorageService> logger)
        {
            _azureSettings = azureSettings.Value;
            _logger = logger;

            // Initialize BlobServiceClient with connection string
            if (!string.IsNullOrEmpty(_azureSettings.BlobStorageConnectionString))
            {
                _blobServiceClient = new BlobServiceClient(_azureSettings.BlobStorageConnectionString);
            }
            else
            {
                _logger.LogWarning("Azure Blob Storage connection string is not configured");
            }
        }

        /// <summary>
        /// Uploads a file to Azure Blob Storage
        /// </summary>
        public async Task<string> UploadFileAsync(string containerName, string fileName, Stream fileStream, string contentType)
        {
            try
            {
                if (_blobServiceClient == null)
                {
                    throw new InvalidOperationException("Blob Storage is not configured properly");
                }

                // Get or create container
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

                // Generate unique file name with timestamp
                var uniqueFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.UtcNow:yyyyMMddHHmmss}{Path.GetExtension(fileName)}";

                // Get blob client
                var blobClient = containerClient.GetBlobClient(uniqueFileName);

                // Set blob HTTP headers
                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = contentType
                };

                // Upload file
                await blobClient.UploadAsync(fileStream, new BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders
                });

                _logger.LogInformation("File uploaded successfully: {FileName} to container: {ContainerName}", uniqueFileName, containerName);

                // Return blob URL
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file {FileName} to container {ContainerName}", fileName, containerName);
                throw;
            }
        }

        /// <summary>
        /// Downloads a file from Azure Blob Storage
        /// </summary>
        public async Task<Stream> DownloadFileAsync(string containerName, string fileName)
        {
            try
            {
                if (_blobServiceClient == null)
                {
                    throw new InvalidOperationException("Blob Storage is not configured properly");
                }

                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);

                // Check if blob exists
                if (!await blobClient.ExistsAsync())
                {
                    throw new FileNotFoundException($"File {fileName} not found in container {containerName}");
                }

                // Download blob to memory stream
                var memoryStream = new MemoryStream();
                await blobClient.DownloadToAsync(memoryStream);
                memoryStream.Position = 0;

                _logger.LogInformation("File downloaded successfully: {FileName} from container: {ContainerName}", fileName, containerName);

                return memoryStream;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file {FileName} from container {ContainerName}", fileName, containerName);
                throw;
            }
        }

        /// <summary>
        /// Deletes a file from Azure Blob Storage
        /// </summary>
        public async Task<bool> DeleteFileAsync(string containerName, string fileName)
        {
            try
            {
                if (_blobServiceClient == null)
                {
                    throw new InvalidOperationException("Blob Storage is not configured properly");
                }

                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);

                // Delete blob if exists
                var response = await blobClient.DeleteIfExistsAsync();

                if (response.Value)
                {
                    _logger.LogInformation("File deleted successfully: {FileName} from container: {ContainerName}", fileName, containerName);
                }
                else
                {
                    _logger.LogWarning("File not found for deletion: {FileName} in container: {ContainerName}", fileName, containerName);
                }

                return response.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file {FileName} from container {ContainerName}", fileName, containerName);
                throw;
            }
        }

        /// <summary>
        /// Checks if a file exists in Azure Blob Storage
        /// </summary>
        public async Task<bool> FileExistsAsync(string containerName, string fileName)
        {
            try
            {
                if (_blobServiceClient == null)
                {
                    throw new InvalidOperationException("Blob Storage is not configured properly");
                }

                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);

                return await blobClient.ExistsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking file existence {FileName} in container {ContainerName}", fileName, containerName);
                throw;
            }
        }
    }
}