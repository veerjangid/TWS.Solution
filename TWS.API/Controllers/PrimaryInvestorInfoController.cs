using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWS.Core.DTOs.Request.PrimaryInvestorInfo;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.PrimaryInvestorInfo;
using TWS.Core.Interfaces.IServices;

namespace TWS.API.Controllers
{
    /// <summary>
    /// Controller for Primary Investor Info management operations
    /// Handles personal information, broker affiliation, investment experience, source of funds, and tax rates
    /// </summary>
    [ApiController]
    [Route("api/primary-investor-info")]
    public class PrimaryInvestorInfoController : ControllerBase
    {
        private readonly IPrimaryInvestorInfoService _primaryInvestorInfoService;
        private readonly ILogger<PrimaryInvestorInfoController> _logger;

        public PrimaryInvestorInfoController(
            IPrimaryInvestorInfoService primaryInvestorInfoService,
            ILogger<PrimaryInvestorInfoController> logger)
        {
            _primaryInvestorInfoService = primaryInvestorInfoService;
            _logger = logger;
        }

        /// <summary>
        /// Save or update Primary Investor Info
        /// Creates new record or updates existing primary investor information
        /// </summary>
        /// <param name="request">Primary investor info request</param>
        /// <returns>Created or updated primary investor info response</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PrimaryInvestorInfoResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<PrimaryInvestorInfoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SavePrimaryInvestorInfo([FromBody] SavePrimaryInvestorInfoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SavePrimaryInvestorInfo");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate age (must be 18+)
                var age = DateTime.UtcNow.Year - request.DateOfBirth.Year;
                if (request.DateOfBirth > DateTime.UtcNow.AddYears(-age)) age--;
                if (age < 18)
                {
                    _logger.LogWarning("Investor must be at least 18 years old. Age: {Age}, InvestorProfileId: {InvestorProfileId}",
                        age, request.InvestorProfileId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Investor must be at least 18 years old",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate driver's license expiration date
                if (request.DriversLicenseExpirationDate <= DateTime.UtcNow)
                {
                    _logger.LogWarning("Driver's license is expired for InvestorProfileId: {InvestorProfileId}", request.InvestorProfileId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Driver's license expiration date must be in the future",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("Saving Primary Investor Info for InvestorProfileId {InvestorProfileId}", request.InvestorProfileId);
                var response = await _primaryInvestorInfoService.SavePrimaryInvestorInfoAsync(request);

                if (response.Success)
                {
                    // Return 201 Created for new records, 200 OK for updates
                    var statusCode = response.Message?.Contains("created", StringComparison.OrdinalIgnoreCase) == true
                        ? StatusCodes.Status201Created
                        : StatusCodes.Status200OK;
                    return StatusCode(statusCode, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Primary Investor Info for InvestorProfileId {InvestorProfileId}", request.InvestorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Save or update Broker Affiliation information
        /// Only one broker affiliation per investor (replaces existing)
        /// </summary>
        /// <param name="request">Broker affiliation request</param>
        /// <returns>Created broker affiliation response</returns>
        [HttpPost("broker-affiliation")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<BrokerAffiliationResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveBrokerAffiliation([FromBody] SaveBrokerAffiliationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SaveBrokerAffiliation");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate conditional fields
                if (request.IsEmployeeOfBrokerDealer && string.IsNullOrWhiteSpace(request.BrokerDealerName))
                {
                    _logger.LogWarning("Broker-dealer name is required when employee flag is true. PrimaryInvestorInfoId: {Id}",
                        request.PrimaryInvestorInfoId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Broker-dealer name is required when investor is an employee of a broker-dealer",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                if (request.IsRelatedToEmployee &&
                    (string.IsNullOrWhiteSpace(request.RelatedBrokerDealerName) ||
                     string.IsNullOrWhiteSpace(request.EmployeeName) ||
                     string.IsNullOrWhiteSpace(request.Relationship)))
                {
                    _logger.LogWarning("Related broker-dealer details are incomplete. PrimaryInvestorInfoId: {Id}",
                        request.PrimaryInvestorInfoId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Related broker-dealer name, employee name, and relationship are required when related to employee flag is true",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("Saving Broker Affiliation for PrimaryInvestorInfoId {PrimaryInvestorInfoId}",
                    request.PrimaryInvestorInfoId);
                var response = await _primaryInvestorInfoService.SaveBrokerAffiliationAsync(request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Broker Affiliation for PrimaryInvestorInfoId {PrimaryInvestorInfoId}",
                    request.PrimaryInvestorInfoId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Save Investment Experience information
        /// Replaces all existing investment experiences for the investor
        /// </summary>
        /// <param name="request">Investment experience request with list of experiences</param>
        /// <returns>Created investment experience list response</returns>
        [HttpPost("investment-experience")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<InvestmentExperienceResponse>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveInvestmentExperience([FromBody] SaveInvestmentExperienceRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SaveInvestmentExperience");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate minimum items
                if (request.Experiences == null || request.Experiences.Count == 0)
                {
                    _logger.LogWarning("At least one investment experience is required. PrimaryInvestorInfoId: {Id}",
                        request.PrimaryInvestorInfoId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "At least one investment experience is required",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate that "Other" asset class has description
                foreach (var experience in request.Experiences)
                {
                    // AssetClass enum: Other = 99 (assuming based on common patterns)
                    if (experience.AssetClass == 99 && string.IsNullOrWhiteSpace(experience.OtherDescription))
                    {
                        _logger.LogWarning("Other description is required when asset class is 'Other'. PrimaryInvestorInfoId: {Id}",
                            request.PrimaryInvestorInfoId);
                        return BadRequest(new ApiResponse<object>
                        {
                            Success = false,
                            Message = "Description is required when asset class is 'Other'",
                            StatusCode = StatusCodes.Status400BadRequest
                        });
                    }
                }

                _logger.LogInformation("Saving Investment Experience for PrimaryInvestorInfoId {PrimaryInvestorInfoId}, Count: {Count}",
                    request.PrimaryInvestorInfoId, request.Experiences.Count);
                var response = await _primaryInvestorInfoService.SaveInvestmentExperienceAsync(request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Investment Experience for PrimaryInvestorInfoId {PrimaryInvestorInfoId}",
                    request.PrimaryInvestorInfoId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Save Source of Funds information
        /// Replaces all existing source of funds for the investor
        /// </summary>
        /// <param name="request">Source of funds request with list of source types</param>
        /// <returns>Created source of funds list response</returns>
        [HttpPost("source-of-funds")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<SourceOfFundsResponse>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveSourceOfFunds([FromBody] SaveSourceOfFundsRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SaveSourceOfFunds");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate minimum items
                if (request.SourceTypes == null || request.SourceTypes.Count == 0)
                {
                    _logger.LogWarning("At least one source of funds is required. PrimaryInvestorInfoId: {Id}",
                        request.PrimaryInvestorInfoId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "At least one source of funds is required",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("Saving Source of Funds for PrimaryInvestorInfoId {PrimaryInvestorInfoId}, Count: {Count}",
                    request.PrimaryInvestorInfoId, request.SourceTypes.Count);
                var response = await _primaryInvestorInfoService.SaveSourceOfFundsAsync(request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Source of Funds for PrimaryInvestorInfoId {PrimaryInvestorInfoId}",
                    request.PrimaryInvestorInfoId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Save Tax Rates information
        /// Replaces all existing tax rates for the investor
        /// </summary>
        /// <param name="request">Tax rates request with list of tax rate ranges</param>
        /// <returns>Created tax rates list response</returns>
        [HttpPost("tax-rates")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<TaxRateResponse>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveTaxRates([FromBody] SaveTaxRatesRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for SaveTaxRates");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = ModelState,
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                // Validate minimum items
                if (request.TaxRateRanges == null || request.TaxRateRanges.Count == 0)
                {
                    _logger.LogWarning("At least one tax rate is required. PrimaryInvestorInfoId: {Id}",
                        request.PrimaryInvestorInfoId);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "At least one tax rate is required",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                _logger.LogInformation("Saving Tax Rates for PrimaryInvestorInfoId {PrimaryInvestorInfoId}, Count: {Count}",
                    request.PrimaryInvestorInfoId, request.TaxRateRanges.Count);
                var response = await _primaryInvestorInfoService.SaveTaxRatesAsync(request);

                if (response.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Tax Rates for PrimaryInvestorInfoId {PrimaryInvestorInfoId}",
                    request.PrimaryInvestorInfoId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Get Primary Investor Info by Investor Profile ID
        /// Returns complete primary investor information with all related data
        /// </summary>
        /// <param name="investorProfileId">Investor Profile ID</param>
        /// <returns>Primary investor info response with broker affiliation, investment experiences, source of funds, and tax rates</returns>
        [HttpGet("investor/{investorProfileId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PrimaryInvestorInfoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPrimaryInvestorInfoByInvestorProfile(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Retrieving Primary Investor Info for InvestorProfileId {InvestorProfileId}", investorProfileId);
                var response = await _primaryInvestorInfoService.GetByInvestorProfileIdAsync(investorProfileId);

                if (response.Success)
                {
                    return Ok(response);
                }

                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Primary Investor Info for InvestorProfileId {InvestorProfileId}", investorProfileId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while processing your request",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}