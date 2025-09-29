using AutoMapper;
using Microsoft.Extensions.Logging;
using TWS.Core.DTOs.Request.Account;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Account;
using TWS.Core.Interfaces.IRepositories;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Entities.Core;

namespace TWS.Infra.Services.Core
{
    /// <summary>
    /// Service implementation for account request operations
    /// Handles business logic for processing account requests from potential investors
    /// Reference: BusinessRequirement.md Section 3.1, FunctionalRequirement.md
    /// </summary>
    public class RequestAccountService : IRequestAccountService
    {
        private readonly IRequestAccountRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<RequestAccountService> _logger;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        public RequestAccountService(
            IRequestAccountRepository repository,
            IMapper mapper,
            ILogger<RequestAccountService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new account request from a potential investor
        /// Checks if email already has unprocessed request before creating
        /// </summary>
        public async Task<ApiResponse<AccountRequestResponse>> CreateRequestAsync(CreateAccountRequestRequest request)
        {
            try
            {
                _logger.LogInformation("Creating new account request for email: {Email}", request.Email);

                // Check if email already has an unprocessed request
                var existingRequest = await _repository.GetByEmailAsync(request.Email);
                if (existingRequest != null)
                {
                    var existing = existingRequest as AccountRequest;
                    if (existing != null && !existing.IsProcessed)
                    {
                        _logger.LogWarning("Email {Email} already has an unprocessed account request", request.Email);
                        return ApiResponse<AccountRequestResponse>.ErrorResponse(
                            "An unprocessed account request already exists for this email address.",
                            400);
                    }
                }

                // Map DTO to entity
                var accountRequest = _mapper.Map<AccountRequest>(request);
                accountRequest.RequestDate = DateTime.UtcNow;
                accountRequest.IsProcessed = false;

                // Save to database
                var createdRequest = await _repository.AddAsync(accountRequest);
                await _repository.SaveChangesAsync();

                _logger.LogInformation("Successfully created account request with ID: {Id}", ((AccountRequest)createdRequest).Id);

                // Map back to response DTO
                var response = _mapper.Map<AccountRequestResponse>((AccountRequest)createdRequest);

                return ApiResponse<AccountRequestResponse>.SuccessResponse(
                    response,
                    "Account request created successfully.",
                    201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account request for email: {Email}", request.Email);
                return ApiResponse<AccountRequestResponse>.ErrorResponse(
                    "An error occurred while creating the account request. Please try again later.",
                    500);
            }
        }

        /// <summary>
        /// Retrieves all account requests ordered by request date descending
        /// </summary>
        public async Task<ApiResponse<List<AccountRequestResponse>>> GetAllRequestsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all account requests");

                var requests = await _repository.GetAllAsync();
                var requestsList = requests.Cast<AccountRequest>()
                    .OrderByDescending(r => r.RequestDate)
                    .ToList();

                var response = _mapper.Map<List<AccountRequestResponse>>(requestsList);

                _logger.LogInformation("Successfully retrieved {Count} account requests", response.Count);

                return ApiResponse<List<AccountRequestResponse>>.SuccessResponse(
                    response,
                    "Account requests retrieved successfully.",
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all account requests");
                return ApiResponse<List<AccountRequestResponse>>.ErrorResponse(
                    "An error occurred while retrieving account requests. Please try again later.",
                    500);
            }
        }

        /// <summary>
        /// Retrieves a specific account request by ID
        /// </summary>
        public async Task<ApiResponse<AccountRequestResponse>> GetRequestByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving account request with ID: {Id}", id);

                var request = await _repository.GetByIdAsync(id);
                if (request == null)
                {
                    _logger.LogWarning("Account request with ID {Id} not found", id);
                    return ApiResponse<AccountRequestResponse>.ErrorResponse(
                        "Account request not found.",
                        404);
                }

                var response = _mapper.Map<AccountRequestResponse>((AccountRequest)request);

                _logger.LogInformation("Successfully retrieved account request with ID: {Id}", id);

                return ApiResponse<AccountRequestResponse>.SuccessResponse(
                    response,
                    "Account request retrieved successfully.",
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving account request with ID: {Id}", id);
                return ApiResponse<AccountRequestResponse>.ErrorResponse(
                    "An error occurred while retrieving the account request. Please try again later.",
                    500);
            }
        }

        /// <summary>
        /// Processes an account request by marking it as processed
        /// Updates ProcessedDate, ProcessedByUserId, and optional notes
        /// </summary>
        public async Task<ApiResponse<AccountRequestResponse>> ProcessRequestAsync(
            int id,
            ProcessAccountRequestRequest request,
            string userId)
        {
            try
            {
                _logger.LogInformation("Processing account request with ID: {Id} by user: {UserId}", id, userId);

                var existingRequest = await _repository.GetByIdAsync(id);
                if (existingRequest == null)
                {
                    _logger.LogWarning("Account request with ID {Id} not found", id);
                    return ApiResponse<AccountRequestResponse>.ErrorResponse(
                        "Account request not found.",
                        404);
                }

                var accountRequest = (AccountRequest)existingRequest;

                if (accountRequest.IsProcessed)
                {
                    _logger.LogWarning("Account request with ID {Id} has already been processed", id);
                    return ApiResponse<AccountRequestResponse>.ErrorResponse(
                        "This account request has already been processed.",
                        400);
                }

                // Update processing details
                accountRequest.IsProcessed = true;
                accountRequest.ProcessedDate = DateTime.UtcNow;
                accountRequest.ProcessedByUserId = userId;
                accountRequest.Notes = request.Notes;

                await _repository.UpdateAsync(accountRequest);
                await _repository.SaveChangesAsync();

                _logger.LogInformation("Successfully processed account request with ID: {Id}", id);

                // Retrieve updated request with navigation properties
                var updatedRequest = await _repository.GetByIdAsync(id);
                var response = _mapper.Map<AccountRequestResponse>((AccountRequest)updatedRequest);

                return ApiResponse<AccountRequestResponse>.SuccessResponse(
                    response,
                    "Account request processed successfully.",
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing account request with ID: {Id}", id);
                return ApiResponse<AccountRequestResponse>.ErrorResponse(
                    "An error occurred while processing the account request. Please try again later.",
                    500);
            }
        }

        /// <summary>
        /// Deletes an account request by ID
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteRequestAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting account request with ID: {Id}", id);

                var request = await _repository.GetByIdAsync(id);
                if (request == null)
                {
                    _logger.LogWarning("Account request with ID {Id} not found", id);
                    return ApiResponse<bool>.ErrorResponse(
                        "Account request not found.",
                        404);
                }

                await _repository.DeleteAsync(request);
                await _repository.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted account request with ID: {Id}", id);

                return ApiResponse<bool>.SuccessResponse(
                    true,
                    "Account request deleted successfully.",
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting account request with ID: {Id}", id);
                return ApiResponse<bool>.ErrorResponse(
                    "An error occurred while deleting the account request. Please try again later.",
                    500);
            }
        }
    }
}