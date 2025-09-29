TWS Investment Platform - Azure Services Implementation in API (Updated)
========================================================================

Document Information
--------------------

*   **Version**: 3.0 (Updated)
*   **Purpose**: Azure services implementation with different authentication methods
*   **Scope**: API project only

* * *

1. Authentication Strategy
--------------------------

### 1.1 Overview

yaml

    Development/QA:
      - ClientSecretCredential (Service Principal)
      - Client ID and Secret in app settings
      
    Staging/Production:
      - DefaultAzureCredential (Managed Identity)
      - No credentials in code

* * *

2. NuGet Packages (All Environments)
------------------------------------

### 2.1 Add to TWS.API Project

xml

    <PackageReference Include="Azure.Identity" Version="1.10.*" />
    <PackageReference Include="Azure.Security.KeyVault.Secrets" Version="4.5.*" />
    <PackageReference Include="Azure.Storage.Blobs" Version="12.19.*" />
    <PackageReference Include="Microsoft.Extensions.Configuration.AzureKeyVault" Version="3.1.*" />
    <PackageReference Include="Serilog.AspNetCore" Version="8.0.*" />
    <PackageReference Include="Serilog.Sinks.File" Version="5.0.*" />

* * *

3. Azure Key Vault Integration
------------------------------

### 3.1 Configure Key Vault in Program.cs

csharp

    // TWS.API/Program.cs
    using Azure.Identity;
    
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
    
            // Configure Serilog for local file logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(
                    path: "logs/api-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30)
                .CreateLogger();
    
            builder.Host.UseSerilog();
    
            // Configure Key Vault based on environment
            ConfigureKeyVault(builder);
    
            builder.Services.AddControllers();
            
            var app = builder.Build();
            
            app.UseSerilogRequestLogging();
            
            app.Run();
        }
        
        private static void ConfigureKeyVault(WebApplicationBuilder builder)
        {
            var keyVaultUrl = builder.Configuration["KeyVault:Uri"];
            if (string.IsNullOrEmpty(keyVaultUrl))
            {
                Log.Warning("Key Vault URI not configured");
                return;
            }
            
            var environment = builder.Environment.EnvironmentName.ToLower();
            
            try
            {
                if (environment == "development" || environment == "qa")
                {
                    // Use Service Principal for Dev/QA
                    var tenantId = builder.Configuration["Azure:TenantId"];
                    var clientId = builder.Configuration["Azure:ClientId"];
                    var clientSecret = builder.Configuration["Azure:ClientSecret"];
                    
                    if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
                    {
                        Log.Error("Azure Service Principal credentials not configured for {Environment}", environment);
                        return;
                    }
                    
                    var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), credential);
                    
                    Log.Information("Key Vault configured with Service Principal for {Environment}", environment);
                }
                else if (environment == "staging" || environment == "production")
                {
                    // Use Managed Identity for Staging/Production
                    var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
                    {
                        ExcludeEnvironmentCredential = true,
                        ExcludeManagedIdentityCredential = false,
                        ExcludeSharedTokenCacheCredential = true,
                        ExcludeVisualStudioCredential = true,
                        ExcludeVisualStudioCodeCredential = true,
                        ExcludeAzureCliCredential = true,
                        ExcludeAzurePowerShellCredential = true,
                        ExcludeInteractiveBrowserCredential = true
                    });
                    
                    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), credential);
                    
                    Log.Information("Key Vault configured with Managed Identity for {Environment}", environment);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to configure Key Vault for {Environment}", environment);
                throw;
            }
        }
    }

* * *

4. Blob Storage Service Implementation
--------------------------------------

### 4.1 Storage Service with Environment-Based Authentication

csharp

    // TWS.Infra/Services/AzureBlobStorageService.cs
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Azure.Storage.Sas;
    using Azure.Identity;
    
    public class AzureBlobStorageService : IStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<AzureBlobStorageService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _environment;
        private const string CONTAINER_NAME = "documents";
        
        public AzureBlobStorageService(
            IConfiguration configuration, 
            ILogger<AzureBlobStorageService> logger,
            IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _logger = logger;
            _environment = environment.EnvironmentName.ToLower();
            
            InitializeBlobServiceClient();
        }
        
        private void InitializeBlobServiceClient()
        {
            try
            {
                var storageAccountName = _configuration["BlobStorage:AccountName"];
                if (string.IsNullOrEmpty(storageAccountName))
                {
                    throw new InvalidOperationException("Storage account name not configured");
                }
                
                var storageUrl = $"https://{storageAccountName}.blob.core.windows.net";
                
                if (_environment == "development" || _environment == "qa")
                {
                    // Use Service Principal for Dev/QA
                    var tenantId = _configuration["Azure:TenantId"];
                    var clientId = _configuration["Azure:ClientId"];
                    var clientSecret = _configuration["Azure:ClientSecret"];
                    
                    if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
                    {
                        // Fallback to connection string if Service Principal not configured
                        var connectionString = _configuration["BlobStorage:ConnectionString"];
                        if (!string.IsNullOrEmpty(connectionString))
                        {
                            _blobServiceClient = new BlobServiceClient(connectionString);
                            _logger.LogInformation("Blob storage initialized with connection string for {Environment}", _environment);
                            return;
                        }
                        
                        throw new InvalidOperationException("Azure Service Principal credentials not configured");
                    }
                    
                    var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                    _blobServiceClient = new BlobServiceClient(new Uri(storageUrl), credential);
                    
                    _logger.LogInformation("Blob storage initialized with Service Principal for {Environment}", _environment);
                }
                else if (_environment == "staging" || _environment == "production")
                {
                    // Use Managed Identity for Staging/Production
                    var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
                    {
                        ExcludeEnvironmentCredential = true,
                        ExcludeManagedIdentityCredential = false,
                        ExcludeSharedTokenCacheCredential = true,
                        ExcludeVisualStudioCredential = true,
                        ExcludeVisualStudioCodeCredential = true,
                        ExcludeAzureCliCredential = true,
                        ExcludeAzurePowerShellCredential = true,
                        ExcludeInteractiveBrowserCredential = true
                    });
                    
                    _blobServiceClient = new BlobServiceClient(new Uri(storageUrl), credential);
                    
                    _logger.LogInformation("Blob storage initialized with Managed Identity for {Environment}", _environment);
                }
                else
                {
                    // Local development fallback
                    var connectionString = _configuration["BlobStorage:ConnectionString"];
                    _blobServiceClient = new BlobServiceClient(connectionString);
                    
                    _logger.LogInformation("Blob storage initialized with connection string for local development");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize blob storage for {Environment}", _environment);
                throw;
            }
        }
        
        public async Task<string> UploadDocumentAsync(Stream fileStream, string fileName, int investorId, string documentType)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(CONTAINER_NAME);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);
                
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                var blobPath = $"investors/{investorId}/{documentType}/{timestamp}_{fileName}";
                
                var blobClient = containerClient.GetBlobClient(blobPath);
                
                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = GetContentType(fileName)
                };
                
                await blobClient.UploadAsync(fileStream, new BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders
                });
                
                _logger.LogInformation("Document uploaded successfully: {BlobPath}", blobPath);
                return blobPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading document");
                throw;
            }
        }
        
        public async Task<string> GenerateSasUrlAsync(string filePath, int expiryMinutes = 5)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(CONTAINER_NAME);
                var blobClient = containerClient.GetBlobClient(filePath);
                
                // For Managed Identity environments, use User Delegation SAS
                if (_environment == "staging" || _environment == "production")
                {
                    return await GenerateUserDelegationSasUrl(blobClient, expiryMinutes);
                }
                else
                {
                    // For Dev/QA, try User Delegation first, fallback to Service SAS
                    try
                    {
                        return await GenerateUserDelegationSasUrl(blobClient, expiryMinutes);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "User delegation SAS failed, trying service SAS");
                        return GenerateServiceSasUrl(blobClient, expiryMinutes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating SAS URL for: {FilePath}", filePath);
                throw;
            }
        }
        
        private async Task<string> GenerateUserDelegationSasUrl(BlobClient blobClient, int expiryMinutes)
        {
            var delegationKey = await _blobServiceClient.GetUserDelegationKeyAsync(
                DateTimeOffset.UtcNow.AddMinutes(-5), 
                DateTimeOffset.UtcNow.AddHours(1));
            
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = CONTAINER_NAME,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(expiryMinutes)
            };
            
            sasBuilder.SetPermissions(BlobSasPermissions.Read);
            
            var blobUriBuilder = new BlobUriBuilder(blobClient.Uri)
            {
                Sas = sasBuilder.ToSasQueryParameters(delegationKey, _blobServiceClient.AccountName)
            };
            
            return blobUriBuilder.ToUri().ToString();
        }
        
        private string GenerateServiceSasUrl(BlobClient blobClient, int expiryMinutes)
        {
            if (!blobClient.CanGenerateSasUri)
            {
                throw new InvalidOperationException("Cannot generate SAS token with current configuration");
            }
            
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = CONTAINER_NAME,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(expiryMinutes)
            };
            
            sasBuilder.SetPermissions(BlobSasPermissions.Read);
            
            return blobClient.GenerateSasUri(sasBuilder).ToString();
        }
        
        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };
        }
    }

* * *

5. Configuration Files
----------------------

### 5.1 appsettings.Development.json

json

    {
      "Azure": {
        "TenantId": "your-tenant-id",
        "ClientId": "your-client-id",
        "ClientSecret": "your-client-secret"
      },
      "KeyVault": {
        "Uri": "https://tws-kv-dev.vault.azure.net/"
      },
      "BlobStorage": {
        "AccountName": "twsstoragedev"
      }
    }

### 5.2 appsettings.qa.json

json

    {
      "Azure": {
        "TenantId": "your-tenant-id",
        "ClientId": "your-client-id",
        "ClientSecret": "your-client-secret"
      },
      "KeyVault": {
        "Uri": "https://tws-kv-qa.vault.azure.net/"
      },
      "BlobStorage": {
        "AccountName": "twsstorageqa"
      }
    }

### 5.3 appsettings.Staging.json

json

    {
      "KeyVault": {
        "Uri": "https://tws-kv-staging.vault.azure.net/"
      },
      "BlobStorage": {
        "AccountName": "twsstoragestaging"
      }
    }

### 5.4 appsettings.Production.json

json

    {
      "KeyVault": {
        "Uri": "https://tws-kv-prod.vault.azure.net/"
      },
      "BlobStorage": {
        "AccountName": "twsstorageprod"
      }
    }

* * *

6. Key Vault Service (Optional Helper)
--------------------------------------

### 6.1 Service Implementation

csharp

    // TWS.Infra/Services/KeyVaultService.cs
    public class KeyVaultService : IKeyVaultService
    {
        private readonly SecretClient _secretClient;
        private readonly ILogger<KeyVaultService> _logger;
        
        public KeyVaultService(
            IConfiguration configuration, 
            ILogger<KeyVaultService> logger,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            
            var keyVaultUrl = configuration["KeyVault:Uri"];
            if (string.IsNullOrEmpty(keyVaultUrl))
            {
                _logger.LogWarning("Key Vault URL not configured");
                return;
            }
            
            var environmentName = environment.EnvironmentName.ToLower();
            
            if (environmentName == "development" || environmentName == "qa")
            {
                // Use Service Principal
                var tenantId = configuration["Azure:TenantId"];
                var clientId = configuration["Azure:ClientId"];
                var clientSecret = configuration["Azure:ClientSecret"];
                
                var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                _secretClient = new SecretClient(new Uri(keyVaultUrl), credential);
            }
            else
            {
                // Use Managed Identity
                var credential = new DefaultAzureCredential();
                _secretClient = new SecretClient(new Uri(keyVaultUrl), credential);
            }
            
            _logger.LogInformation("Key Vault client initialized for {Environment}", environmentName);
        }
        
        public async Task<string> GetSecretAsync(string secretName)
        {
            try
            {
                var secret = await _secretClient.GetSecretAsync(secretName);
                return secret.Value.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving secret: {SecretName}", secretName);
                throw;
            }
        }
    }

* * *

7. Service Registration
-----------------------

### 7.1 Program.cs Service Registration

csharp

    // TWS.API/Program.cs
    var builder = WebApplication.CreateBuilder(args);
    
    // Register services
    builder.Services.AddSingleton<IStorageService, AzureBlobStorageService>();
    builder.Services.AddSingleton<IDataProtectionService, DataProtectionService>();
    builder.Services.AddSingleton<IKeyVaultService, KeyVaultService>();
    
    // Add DbContext - connection string will come from Key Vault
    builder.Services.AddDbContext<TWSDbContext>(options =>
        options.UseMySql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

* * *

8. Implementation Checklist
---------------------------

### 8.1 Development Environment

*    Create Service Principal for Dev
*    Add Service Principal credentials to appsettings.Development.json
*    Configure Key Vault access for Service Principal
*    Configure Storage access for Service Principal
*    Test Key Vault connection
*    Test Blob Storage upload/download

### 8.2 QA Environment

*    Create Service Principal for QA
*    Add Service Principal credentials to App Service settings
*    Configure Key Vault access for Service Principal
*    Configure Storage access for Service Principal
*    Deploy and test

### 8.3 Staging/Production Environment

*    Enable Managed Identity on API App Service
*    Grant Key Vault access to Managed Identity
*    Grant Storage access to Managed Identity
*    No credentials in app settings
*    Deploy and test

* * *

9. Security Notes
-----------------

### 9.1 Service Principal Credentials

yaml

    Development:
      - Can be in appsettings.Development.json for local development
      - Use user secrets for better security locally
    
    QA:
      - Should be in App Service Application Settings
      - Never commit to source control
    
    Staging/Production:
      - No credentials needed (Managed Identity)
      - Most secure option

### 9.2 Secret Management

bash

    # For local development, use Secret Manager
    dotnet user-secrets init
    dotnet user-secrets set "Azure:ClientId" "your-client-id"
    dotnet user-secrets set "Azure:ClientSecret" "your-client-secret"
    dotnet user-secrets set "Azure:TenantId" "your-tenant-id"

* * *

**END OF AZURE IMPLEMENTATION GUIDE**
_Note: Dev/QA environments use Service Principal authentication with Client ID/Secret. Staging/Production use Managed Identity for enhanced security._