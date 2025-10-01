using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TWS.Core.DTOs.Request.Offering;
using TWS.Core.DTOs.Response.Investment;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    /// <summary>
    /// Manages investment offerings.
    /// Provides endpoints to view available investment opportunities.
    /// Reference: APIDoc.md Section 12
    /// </summary>
    [ApiController]
    [Route("api/offerings")]
    [Authorize]
    public class OfferingController : ControllerBase
    {
        private readonly IOfferingService _offeringService;
        private readonly ILogger<OfferingController> _logger;

        public OfferingController(
            IOfferingService offeringService,
            ILogger<OfferingController> logger)
        {
            _offeringService = offeringService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all investment offerings.
        /// Returns all offerings regardless of status.
        /// </summary>
        /// <returns>Collection of all offerings</returns>
        /// <response code="200">Successfully retrieved offerings</response>
        /// <response code="401">Unauthorized - authentication required</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OfferingResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<OfferingResponse>>> GetAllOfferings()
        {
            try
            {
                _logger.LogInformation("Retrieving all offerings");

                var offerings = await _offeringService.GetAllOfferingsAsync();

                return Ok(offerings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all offerings");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving offerings" });
            }
        }

        /// <summary>
        /// Gets all active investment offerings.
        /// Returns only offerings with status "Raising" (currently accepting investments).
        /// </summary>
        /// <returns>Collection of active offerings</returns>
        /// <response code="200">Successfully retrieved active offerings</response>
        /// <response code="401">Unauthorized - authentication required</response>
        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<OfferingResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<OfferingResponse>>> GetActiveOfferings()
        {
            try
            {
                _logger.LogInformation("Retrieving active offerings");

                var offerings = await _offeringService.GetActiveOfferingsAsync();

                return Ok(offerings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active offerings");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving active offerings" });
            }
        }

        /// <summary>
        /// Gets an investment offering by ID.
        /// Returns detailed information about a specific offering.
        /// </summary>
        /// <param name="id">Offering ID</param>
        /// <returns>Offering details</returns>
        /// <response code="200">Successfully retrieved offering</response>
        /// <response code="404">Offering not found</response>
        /// <response code="401">Unauthorized - authentication required</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OfferingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OfferingResponse>> GetOfferingById(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving offering {OfferingId}", id);

                var offering = await _offeringService.GetByIdAsync(id);

                if (offering == null)
                {
                    _logger.LogWarning("Offering {OfferingId} not found", id);
                    return NotFound(new { message = $"Offering with ID {id} not found" });
                }

                return Ok(offering);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving offering {OfferingId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving the offering" });
            }
        }

        /// <summary>
        /// Creates a new investment offering.
        /// Restricted to Advisors and Operations Team only.
        /// </summary>
        /// <param name="request">Offering creation details</param>
        /// <returns>Created offering details</returns>
        /// <response code="201">Successfully created offering</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="403">Forbidden - insufficient permissions</response>
        [HttpPost]
        [Authorize(Roles = "Advisor,OperationsTeam")]
        [ProducesResponseType(typeof(OfferingResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> CreateOffering([FromBody] CreateOfferingRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in token" });
                }

                _logger.LogInformation("Creating offering: {OfferingName} by user {UserId}", request.Name, userId);

                var response = await _offeringService.CreateOfferingAsync(request, userId);

                if (!response.Success)
                {
                    return StatusCode(response.StatusCode, new { message = response.Message });
                }

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating offering");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while creating the offering" });
            }
        }

        /// <summary>
        /// Updates an existing investment offering.
        /// Restricted to Advisors and Operations Team only.
        /// </summary>
        /// <param name="id">Offering ID to update</param>
        /// <param name="request">Updated offering details</param>
        /// <returns>Updated offering details</returns>
        /// <response code="200">Successfully updated offering</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="404">Offering not found</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="403">Forbidden - insufficient permissions</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Advisor,OperationsTeam")]
        [ProducesResponseType(typeof(OfferingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UpdateOffering(int id, [FromBody] UpdateOfferingRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in token" });
                }

                _logger.LogInformation("Updating offering {OfferingId} by user {UserId}", id, userId);

                var response = await _offeringService.UpdateOfferingAsync(id, request, userId);

                if (!response.Success)
                {
                    return StatusCode(response.StatusCode, new { message = response.Message });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating offering {OfferingId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while updating the offering" });
            }
        }

        /// <summary>
        /// Deletes an investment offering.
        /// Restricted to Advisors and Operations Team only.
        /// </summary>
        /// <param name="id">Offering ID to delete</param>
        /// <returns>Deletion confirmation</returns>
        /// <response code="200">Successfully deleted offering</response>
        /// <response code="404">Offering not found</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="403">Forbidden - insufficient permissions</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Advisor,OperationsTeam")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteOffering(int id)
        {
            try
            {
                _logger.LogInformation("Deleting offering {OfferingId}", id);

                var response = await _offeringService.DeleteOfferingAsync(id);

                if (!response.Success)
                {
                    return StatusCode(response.StatusCode, new { message = response.Message });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting offering {OfferingId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while deleting the offering" });
            }
        }

        /// <summary>
        /// Uploads an image for an offering.
        /// Restricted to Advisors and Operations Team only.
        /// </summary>
        /// <param name="id">Offering ID</param>
        /// <param name="file">Image file to upload</param>
        /// <returns>Image path</returns>
        /// <response code="200">Successfully uploaded image</response>
        /// <response code="400">Invalid file</response>
        /// <response code="404">Offering not found</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="403">Forbidden - insufficient permissions</response>
        [HttpPost("{id}/upload-image")]
        [Authorize(Roles = "Advisor,OperationsTeam")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UploadImage(int id, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "No file uploaded" });
                }

                _logger.LogInformation("Uploading image for offering {OfferingId}", id);

                var imagePath = $"/uploads/offerings/{id}/images/{Guid.NewGuid()}_{file.FileName}";

                var response = await _offeringService.UploadImageAsync(id, imagePath);

                if (!response.Success)
                {
                    return StatusCode(response.StatusCode, new { message = response.Message });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image for offering {OfferingId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while uploading the image" });
            }
        }

        /// <summary>
        /// Uploads a PDF for an offering.
        /// Restricted to Advisors and Operations Team only.
        /// </summary>
        /// <param name="id">Offering ID</param>
        /// <param name="file">PDF file to upload</param>
        /// <returns>PDF path</returns>
        /// <response code="200">Successfully uploaded PDF</response>
        /// <response code="400">Invalid file</response>
        /// <response code="404">Offering not found</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="403">Forbidden - insufficient permissions</response>
        [HttpPost("{id}/upload-pdf")]
        [Authorize(Roles = "Advisor,OperationsTeam")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UploadPDF(int id, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "No file uploaded" });
                }

                _logger.LogInformation("Uploading PDF for offering {OfferingId}", id);

                var pdfPath = $"/uploads/offerings/{id}/pdfs/{Guid.NewGuid()}_{file.FileName}";

                var response = await _offeringService.UploadPDFAsync(id, pdfPath);

                if (!response.Success)
                {
                    return StatusCode(response.StatusCode, new { message = response.Message });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading PDF for offering {OfferingId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while uploading the PDF" });
            }
        }

        /// <summary>
        /// Uploads an additional document for an offering.
        /// Restricted to Advisors and Operations Team only.
        /// </summary>
        /// <param name="id">Offering ID</param>
        /// <param name="request">Document upload request</param>
        /// <param name="file">Document file to upload</param>
        /// <returns>Document details</returns>
        /// <response code="201">Successfully uploaded document</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">Offering not found</response>
        /// <response code="401">Unauthorized - authentication required</response>
        /// <response code="403">Forbidden - insufficient permissions</response>
        [HttpPost("{id}/upload-document")]
        [Authorize(Roles = "Advisor,OperationsTeam")]
        [ProducesResponseType(typeof(OfferingDocumentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UploadDocument(int id, [FromForm] UploadOfferingDocumentRequest request, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "No file uploaded" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Uploading document for offering {OfferingId}", id);

                var filePath = $"/uploads/offerings/{id}/documents/{Guid.NewGuid()}_{file.FileName}";

                var response = await _offeringService.UploadDocumentAsync(id, request.DocumentName, filePath);

                if (!response.Success)
                {
                    return StatusCode(response.StatusCode, new { message = response.Message });
                }

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading document for offering {OfferingId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while uploading the document" });
            }
        }

        /// <summary>
        /// Gets all documents for an offering.
        /// Returns all additional documents associated with the offering.
        /// </summary>
        /// <param name="id">Offering ID</param>
        /// <returns>Collection of offering documents</returns>
        /// <response code="200">Successfully retrieved documents</response>
        /// <response code="404">Offering not found</response>
        /// <response code="401">Unauthorized - authentication required</response>
        [HttpGet("{id}/documents")]
        [ProducesResponseType(typeof(List<OfferingDocumentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetOfferingDocuments(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving documents for offering {OfferingId}", id);

                var response = await _offeringService.GetOfferingDocumentsAsync(id);

                if (!response.Success)
                {
                    return StatusCode(response.StatusCode, new { message = response.Message });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving documents for offering {OfferingId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving the documents" });
            }
        }
    }
}