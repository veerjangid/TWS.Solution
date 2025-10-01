# Azure Integration Migration Guide

This document provides guidance on integrating the newly implemented Azure services into existing TWS services.

## Overview

Three Azure services have been implemented:
1. **Blob Storage Service** - For cloud-based file storage
2. **Key Vault Service** - For secure secret management
3. **Encryption Service** - For AES-256 field-level encryption

## Configuration

### Development Environment

The default configuration in `appsettings.json` uses local/development settings:

```json
{
  "AzureSettings": {
    "BlobStorageConnectionString": "UseDevelopmentStorage=true",
    "BlobStorageContainerName": "tws-documents",
    "KeyVaultUrl": "https://your-keyvault.vault.azure.net/",
    "UseBlobStorage": false
  },
  "EncryptionSettings": {
    "EncryptionKey": "DevKeyMinimum32CharactersLongForAES256!",
    "UseKeyVault": false,
    "KeyVaultSecretName": "EncryptionKey"
  }
}
```

### Production Environment

For production, update `appsettings.Production.json`:

```json
{
  "AzureSettings": {
    "BlobStorageConnectionString": "DefaultEndpointsProtocol=https;AccountName=youraccountname;AccountKey=youraccountkey;EndpointSuffix=core.windows.net",
    "BlobStorageContainerName": "tws-documents",
    "KeyVaultUrl": "https://tws-keyvault.vault.azure.net/",
    "UseBlobStorage": true
  },
  "EncryptionSettings": {
    "EncryptionKey": "",
    "UseKeyVault": true,
    "KeyVaultSecretName": "EncryptionKey"
  }
}
```

## Migration Steps

### 1. Migrating File Upload Services to Azure Blob Storage

The following services currently use local file storage and should be migrated:

#### DocumentService (TWS.Infra/Services/Core/DocumentService.cs)

**Current Implementation:**
- Saves files to local `wwwroot/uploads/documents` folder
- Returns local file paths

**Migration Steps:**
1. Inject `IBlobStorageService` and `AzureSettings` into the constructor
2. Check `AzureSettings.UseBlobStorage` flag
3. If true, use `IBlobStorageService.UploadFileAsync()` instead of local file save
4. Store the returned blob URL in the database instead of local path
5. Keep local file storage as fallback when flag is false

**Example Code:**

```csharp
public class DocumentService : IDocumentService
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly AzureSettings _azureSettings;

    public DocumentService(
        IBlobStorageService blobStorageService,
        IOptions<AzureSettings> azureSettings)
    {
        _blobStorageService = blobStorageService;
        _azureSettings = azureSettings.Value;
    }

    public async Task<string> UploadDocumentAsync(IFormFile file, string containerName = "documents")
    {
        if (_azureSettings.UseBlobStorage)
        {
            // Upload to Azure Blob Storage
            using var stream = file.OpenReadStream();
            var blobUrl = await _blobStorageService.UploadFileAsync(
                containerName,
                file.FileName,
                stream,
                file.ContentType);
            return blobUrl;
        }
        else
        {
            // Fallback to local storage
            var uploadsFolder = Path.Combine("wwwroot", "uploads", containerName);
            // ... existing local save logic
        }
    }
}
```

#### AccreditationService (TWS.Infra/Services/Core/AccreditationService.cs)

**Files to Migrate:**
- Income verification documents
- Net worth verification documents
- Professional certification documents

**Migration Pattern:** Same as DocumentService above

#### GeneralInfoService (TWS.Infra/Services/Core/GeneralInfoService.cs)

**Files to Migrate:**
- Proof of identity documents
- IRA custodian documents

**Migration Pattern:** Same as DocumentService above

#### PersonalFinancialStatementService (TWS.Infra/Services/Financial/PersonalFinancialStatementService.cs)

**Files to Migrate:**
- Financial statement documents
- Supporting documentation

**Migration Pattern:** Same as DocumentService above

### 2. Implementing Field-Level Encryption

The following fields require AES-256 encryption per SecurityDesign.md:

#### Entities Requiring Encryption

**InvestorProfile:**
- `TaxIdentificationNumber` (SSN/EIN)

**IndividualInvestorDetail:**
- `SocialSecurityNumber`

**EntityInvestorDetail:**
- `EmployerIdentificationNumber`

**TrustInvestorDetail:**
- `TaxIdentificationNumber`

**Beneficiary:**
- `SocialSecurityNumber`

**BankAccountDetails (if exists):**
- `AccountNumber`
- `RoutingNumber`

#### Implementation Steps

1. **Inject IEncryptionService** into the relevant service constructors
2. **Encrypt on Save/Update:**

```csharp
public async Task SaveInvestorProfileAsync(InvestorProfile profile)
{
    // Encrypt sensitive fields before saving
    if (!string.IsNullOrEmpty(profile.TaxIdentificationNumber))
    {
        profile.TaxIdentificationNumber = await _encryptionService.EncryptAsync(
            profile.TaxIdentificationNumber);
    }

    await _repository.AddAsync(profile);
    await _repository.SaveAsync();
}
```

3. **Decrypt on Retrieve:**

```csharp
public async Task<InvestorProfileResponse> GetInvestorProfileAsync(int investorId)
{
    var profile = await _repository.GetByIdAsync(investorId);

    // Decrypt sensitive fields
    if (!string.IsNullOrEmpty(profile.TaxIdentificationNumber))
    {
        profile.TaxIdentificationNumber = await _encryptionService.DecryptAsync(
            profile.TaxIdentificationNumber);
    }

    return _mapper.Map<InvestorProfileResponse>(profile);
}
```

4. **Mask in Display (Optional):**

For display purposes, use the `MaskSSN` method:

```csharp
public string GetMaskedSSN(string ssn)
{
    // First decrypt
    var decrypted = await _encryptionService.DecryptAsync(ssn);

    // Then mask for display
    return _encryptionService.MaskSSN(decrypted);
    // Returns: ***-**-6789
}
```

### 3. Database Migration Considerations

**Important:** Existing unencrypted data in the database needs to be encrypted.

Create a one-time migration script:

```csharp
// Example migration script
public async Task MigrateExistingDataToEncryptedAsync()
{
    var profiles = await _repository.GetAllAsync();

    foreach (var profile in profiles)
    {
        if (!string.IsNullOrEmpty(profile.TaxIdentificationNumber) &&
            !IsEncrypted(profile.TaxIdentificationNumber))
        {
            profile.TaxIdentificationNumber = await _encryptionService.EncryptAsync(
                profile.TaxIdentificationNumber);
            await _repository.UpdateAsync(profile);
        }
    }

    await _repository.SaveAsync();
}

private bool IsEncrypted(string value)
{
    // Check if value is already encrypted (Base64 format)
    try
    {
        Convert.FromBase64String(value);
        return value.Length > 50; // Encrypted values are typically longer
    }
    catch
    {
        return false;
    }
}
```

## Azure Resource Setup

### 1. Azure Storage Account

```bash
# Create resource group
az group create --name tws-resources --location eastus

# Create storage account
az storage account create \
  --name twsstorage \
  --resource-group tws-resources \
  --location eastus \
  --sku Standard_LRS

# Create container
az storage container create \
  --name tws-documents \
  --account-name twsstorage \
  --public-access off
```

### 2. Azure Key Vault

```bash
# Create Key Vault
az keyvault create \
  --name tws-keyvault \
  --resource-group tws-resources \
  --location eastus

# Store encryption key
az keyvault secret set \
  --vault-name tws-keyvault \
  --name EncryptionKey \
  --value "YourProductionEncryptionKey32Chars!"
```

### 3. Azure Managed Identity (for Production)

```bash
# Create managed identity
az identity create \
  --name tws-api-identity \
  --resource-group tws-resources

# Grant permissions to Key Vault
az keyvault set-policy \
  --name tws-keyvault \
  --object-id <managed-identity-object-id> \
  --secret-permissions get list

# Grant permissions to Storage Account
az role assignment create \
  --role "Storage Blob Data Contributor" \
  --assignee <managed-identity-object-id> \
  --scope /subscriptions/<subscription-id>/resourceGroups/tws-resources/providers/Microsoft.Storage/storageAccounts/twsstorage
```

## Testing

### Local Testing with Azure Storage Emulator (Azurite)

1. Install Azurite:
```bash
npm install -g azurite
```

2. Start Azurite:
```bash
azurite --silent --location c:\azurite --debug c:\azurite\debug.log
```

3. Set `UseBlobStorage: true` in appsettings.Development.json

### Testing Encryption

```csharp
[Fact]
public async Task EncryptDecrypt_ShouldReturnOriginalValue()
{
    // Arrange
    var plainText = "123-45-6789";

    // Act
    var encrypted = await _encryptionService.EncryptAsync(plainText);
    var decrypted = await _encryptionService.DecryptAsync(encrypted);

    // Assert
    Assert.NotEqual(plainText, encrypted);
    Assert.Equal(plainText, decrypted);
}

[Fact]
public void MaskSSN_ShouldHideFirstFiveDigits()
{
    // Arrange
    var ssn = "123-45-6789";

    // Act
    var masked = _encryptionService.MaskSSN(ssn);

    // Assert
    Assert.Equal("***-**-6789", masked);
}
```

## Rollback Plan

If issues occur with Azure services:

1. Set `UseBlobStorage: false` in appsettings.json
2. Set `UseKeyVault: false` in appsettings.json
3. System will automatically fall back to local storage and configuration-based encryption
4. No code changes required

## Performance Considerations

1. **Key Vault Caching:** The KeyVaultService caches secrets to minimize API calls
2. **Blob Storage:** Use async operations to avoid blocking
3. **Encryption:** Encryption adds ~5-10ms per field operation

## Security Best Practices

1. **Never commit production keys** to source control
2. **Rotate encryption keys** regularly (requires re-encryption of data)
3. **Use Managed Identity** in Azure for authentication
4. **Enable soft delete** on Key Vault for disaster recovery
5. **Enable blob versioning** in Storage Account for data recovery
6. **Audit access logs** regularly for both Key Vault and Storage Account

## Support

For issues or questions:
- Review SecurityDesign.md for encryption requirements
- Review AzureImplementation.md for Azure integration patterns
- Check Azure service health at https://status.azure.com
- Review application logs in Logs/ folder

---

**Last Updated:** Phase 15 Implementation
**Document Version:** 1.0