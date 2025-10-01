using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Document;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    /// <summary>
    /// Manages document uploads for investor profiles.
    /// Handles document storage, retrieval, and deletion.
    /// Reference: APIDoc.md Section 10, BusinessRequirement.md Section 11
    /// </summary>
    [ApiController]
    [Route("api/documents")]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly ILogger<DocumentController> _logger;
        private readonly IWebHostEnvironment _environment;
        private const long MaxFileSize = 10485760; // 10MB in bytes

        public DocumentController(
            IDocumentService documentService,
            ILogger<DocumentController> logger,
            IWebHostEnvironment environment)
        {
            _documentService = documentService;
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// Uploads a single document for an investor profile.
        /// Accepts any file type with a maximum size of 10MB.
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <param name="documentName">Display name for the document</param>
        /// <param name="file">The file to upload</param>
        /// <returns>Uploaded document information</returns>
        /// <response code="201">Document successfully uploaded</response>
        /// <response code="400">Validation failed or file size exceeds limit</response>
        /// <response code="401">Unauthorized - authentication required</response>
        [HttpPost("upload")]
        [ProducesResponseType(typeof(ApiResponse<DocumentResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<DocumentResponse>>> UploadDocument(
            [FromForm] int investorProfileId,
            [FromForm] string documentName,
            [FromForm] IFormFile file)
        {
            try
            {
                // Validate inputs
                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("No file provided for document upload");
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "No file provided",
                        StatusCodes.Status400BadRequest));
                }

                if (file.Length > MaxFileSize)
                {
                    _logger.LogWarning("File size {FileSize} exceeds maximum {MaxSize}", file.Length, MaxFileSize);
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        $"File size exceeds maximum allowed size of {MaxFileSize / 1048576}MB",
                        StatusCodes.Status400BadRequest));
                }

                if (string.IsNullOrWhiteSpace(documentName))
                {
                    _logger.LogWarning("Document name is required");
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "Document name is required",
                        StatusCodes.Status400BadRequest));
                }

                _logger.LogInformation(
                    "Uploading document '{DocumentName}' for InvestorProfileId: {InvestorProfileId}",
                    documentName,
                    investorProfileId);

                // Create directory if it doesn't exist
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "documents", investorProfileId.ToString());
                Directory.CreateDirectory(uploadsFolder);

                // Generate unique filename
                var fileExtension = Path.GetExtension(file.FileName);
                var uniqueFileName = $"{documentName}_{DateTime.UtcNow:yyyyMMddHHmmss}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save file to disk
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation("File saved to: {FilePath}", filePath);

                // Save to database
                var response = await _documentService.UploadDocumentAsync(
                    investorProfileId,
                    documentName,
                    filePath,
                    file.FileName,
                    file.Length,
                    file.ContentType);

                if (!response.Success)
                {
                    // Delete uploaded file if database save failed
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    _logger.LogWarning("Failed to save document metadata: {Message}", response.Message);
                    return BadRequest(response);
                }

                _logger.LogInformation("Document uploaded successfully: {DocumentId}", response.Data?.Id);
                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading document for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "An error occurred while uploading the document",
                        StatusCodes.Status500InternalServerError));
            }
        }

        /// <summary>
        /// Uploads multiple documents for an investor profile.
        /// Accepts any file types with a maximum size of 10MB per file.
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <param name="documentNames">Comma-separated list of document names (must match file count)</param>
        /// <param name="files">The files to upload</param>
        /// <returns>List of uploaded document information</returns>
        /// <response code="201">Documents successfully uploaded</response>
        /// <response code="400">Validation failed or file sizes exceed limit</response>
        /// <response code="401">Unauthorized - authentication required</response>
        [HttpPost("upload-multiple")]
        [ProducesResponseType(typeof(ApiResponse<List<DocumentResponse>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<List<DocumentResponse>>>> UploadMultipleDocuments(
            [FromForm] int investorProfileId,
            [FromForm] string documentNames,
            [FromForm] List<IFormFile> files)
        {
            try
            {
                // Validate inputs
                if (files == null || files.Count == 0)
                {
                    _logger.LogWarning("No files provided for document upload");
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "No files provided",
                        StatusCodes.Status400BadRequest));
                }

                if (string.IsNullOrWhiteSpace(documentNames))
                {
                    _logger.LogWarning("Document names are required");
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "Document names are required",
                        StatusCodes.Status400BadRequest));
                }

                // Parse document names
                var namesList = documentNames.Split(',').Select(n => n.Trim()).ToList();

                if (namesList.Count != files.Count)
                {
                    _logger.LogWarning("Document names count {NamesCount} does not match files count {FilesCount}",
                        namesList.Count, files.Count);
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "Document names count must match files count",
                        StatusCodes.Status400BadRequest));
                }

                // Validate file sizes
                foreach (var file in files)
                {
                    if (file.Length > MaxFileSize)
                    {
                        _logger.LogWarning("File {FileName} size {FileSize} exceeds maximum {MaxSize}",
                            file.FileName, file.Length, MaxFileSize);
                        return BadRequest(ApiResponse<object>.ErrorResponse(
                            $"File {file.FileName} size exceeds maximum allowed size of {MaxFileSize / 1048576}MB",
                            StatusCodes.Status400BadRequest));
                    }
                }

                _logger.LogInformation(
                    "Uploading {Count} documents for InvestorProfileId: {InvestorProfileId}",
                    files.Count,
                    investorProfileId);

                // Create directory if it doesn't exist
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "documents", investorProfileId.ToString());
                Directory.CreateDirectory(uploadsFolder);

                // Save files and prepare document info
                var uploadedDocuments = new List<(string documentName, string filePath, string fileName, long? fileSize, string? contentType)>();
                var savedFilePaths = new List<string>();

                try
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        var documentName = namesList[i];

                        // Generate unique filename
                        var fileExtension = Path.GetExtension(file.FileName);
                        var uniqueFileName = $"{documentName}_{DateTime.UtcNow:yyyyMMddHHmmss}_{i}{fileExtension}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save file to disk
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        savedFilePaths.Add(filePath);
                        uploadedDocuments.Add((documentName, filePath, file.FileName, file.Length, file.ContentType));

                        _logger.LogInformation("File {Index} saved to: {FilePath}", i + 1, filePath);
                    }

                    // Save to database
                    var response = await _documentService.UploadMultipleDocumentsAsync(
                        investorProfileId,
                        uploadedDocuments);

                    if (!response.Success)
                    {
                        // Delete all uploaded files if database save failed
                        foreach (var filePath in savedFilePaths)
                        {
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }

                        _logger.LogWarning("Failed to save documents metadata: {Message}", response.Message);
                        return BadRequest(response);
                    }

                    _logger.LogInformation("{Count} documents uploaded successfully", files.Count);
                    return StatusCode(StatusCodes.Status201Created, response);
                }
                catch
                {
                    // Clean up files on error
                    foreach (var filePath in savedFilePaths)
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading multiple documents for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "An error occurred while uploading the documents",
                        StatusCodes.Status500InternalServerError));
            }
        }

        /// <summary>
        /// Gets all documents for an investor profile.
        /// Returns documents ordered by upload date (newest first).
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <returns>List of documents for the investor</returns>
        /// <response code="200">Documents retrieved successfully</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="404">Investor profile not found</response>
        [HttpGet("investor/{investorProfileId}")]
        [ProducesResponseType(typeof(ApiResponse<List<DocumentResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<List<DocumentResponse>>>> GetByInvestorProfileId(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Retrieving documents for InvestorProfileId: {InvestorProfileId}", investorProfileId);

                var response = await _documentService.GetByInvestorProfileIdAsync(investorProfileId);

                if (!response.Success)
                {
                    _logger.LogWarning("Failed to retrieve documents: {Message}", response.Message);
                    return response.StatusCode == 404 ? NotFound(response) : BadRequest(response);
                }

                _logger.LogInformation("Retrieved {Count} documents for InvestorProfileId: {InvestorProfileId}",
                    response.Data?.Count ?? 0, investorProfileId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving documents for InvestorProfileId: {InvestorProfileId}", investorProfileId);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "An error occurred while retrieving documents",
                        StatusCodes.Status500InternalServerError));
            }
        }

        /// <summary>
        /// Deletes a document by ID.
        /// Removes both the database record and the physical file.
        /// </summary>
        /// <param name="documentId">The document ID</param>
        /// <returns>Success status</returns>
        /// <response code="200">Document successfully deleted</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="404">Document not found</response>
        [HttpDelete("{documentId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteDocument(int documentId)
        {
            try
            {
                _logger.LogInformation("Deleting document: {DocumentId}", documentId);

                var response = await _documentService.DeleteDocumentAsync(documentId);

                if (!response.Success)
                {
                    _logger.LogWarning("Failed to delete document: {Message}", response.Message);
                    return response.StatusCode == 404 ? NotFound(response) : BadRequest(response);
                }

                _logger.LogInformation("Document deleted successfully: {DocumentId}", documentId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting document: {DocumentId}", documentId);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "An error occurred while deleting the document",
                        StatusCodes.Status500InternalServerError));
            }
        }
    }
}