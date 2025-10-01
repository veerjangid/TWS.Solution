using AutoMapper;
using Microsoft.Extensions.Logging;
using TWS.Core.DTOs.Request.Portal;
using TWS.Core.DTOs.Response;
using TWS.Core.DTOs.Response.Portal;
using TWS.Core.Interfaces.IRepositories;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Entities.Portal;

namespace TWS.Infra.Services.Portal
{
    /// <summary>
    /// Service implementation for Portal/CRM module
    /// Handles business logic for investment tracking with financial metrics
    /// Reference: APIDoc.md Section 13, DatabaseSchema.md Table 32
    /// </summary>
    public class PortalService : IPortalService
    {
        private readonly IInvestmentTrackerRepository _trackerRepository;
        private readonly IOfferingRepository _offeringRepository;
        private readonly IInvestorProfileRepository _investorProfileRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PortalService> _logger;

        public PortalService(
            IInvestmentTrackerRepository trackerRepository,
            IOfferingRepository offeringRepository,
            IInvestorProfileRepository investorProfileRepository,
            IMapper mapper,
            ILogger<PortalService> logger)
        {
            _trackerRepository = trackerRepository;
            _offeringRepository = offeringRepository;
            _investorProfileRepository = investorProfileRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new investment tracker
        /// </summary>
        public async Task<ApiResponse<InvestmentTrackerResponse>> CreateTrackerAsync(CreateInvestmentTrackerRequest request)
        {
            try
            {
                _logger.LogInformation("Creating investment tracker for Offering {OfferingId} and InvestorProfile {InvestorProfileId}",
                    request.OfferingId, request.InvestorProfileId);

                // Validate offering exists
                var offering = await _offeringRepository.GetByIdAsync(request.OfferingId);
                if (offering == null)
                {
                    _logger.LogWarning("Offering {OfferingId} not found", request.OfferingId);
                    return ApiResponse<InvestmentTrackerResponse>.ErrorResponse(
                        $"Offering with ID {request.OfferingId} not found",
                        404
                    );
                }

                // Validate investor profile exists
                var investorProfile = await _investorProfileRepository.GetByIdAsync(request.InvestorProfileId);
                if (investorProfile == null)
                {
                    _logger.LogWarning("InvestorProfile {InvestorProfileId} not found", request.InvestorProfileId);
                    return ApiResponse<InvestmentTrackerResponse>.ErrorResponse(
                        $"Investor profile with ID {request.InvestorProfileId} not found",
                        404
                    );
                }

                // Map request to entity
                var tracker = _mapper.Map<InvestmentTracker>(request);

                // Add tracker
                var createdTracker = await _trackerRepository.AddAsync(tracker);
                await _trackerRepository.SaveChangesAsync();

                // Cast to InvestmentTracker to get Id
                var trackerEntity = createdTracker as InvestmentTracker;
                if (trackerEntity == null)
                {
                    _logger.LogError("Failed to cast created tracker to InvestmentTracker");
                    return ApiResponse<InvestmentTrackerResponse>.ErrorResponse(
                        "Internal error: Invalid tracker type",
                        500
                    );
                }

                // Retrieve created tracker with navigation properties
                var trackerWithDetails = await _trackerRepository.GetByIdAsync(trackerEntity.Id);
                var response = _mapper.Map<InvestmentTrackerResponse>(trackerWithDetails);

                _logger.LogInformation("Investment tracker {TrackerId} created successfully", trackerEntity.Id);

                return ApiResponse<InvestmentTrackerResponse>.SuccessResponse(
                    response,
                    "Investment tracker created successfully",
                    201
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating investment tracker");
                return ApiResponse<InvestmentTrackerResponse>.ErrorResponse(
                    "An error occurred while creating the investment tracker",
                    500
                );
            }
        }

        /// <summary>
        /// Updates the status of an investment tracker
        /// </summary>
        public async Task<ApiResponse<InvestmentTrackerResponse>> UpdateStatusAsync(int id, UpdateTrackerStatusRequest request)
        {
            try
            {
                _logger.LogInformation("Updating status for tracker {TrackerId}", id);

                // Check if tracker exists
                var existingTracker = await _trackerRepository.GetByIdAsync(id);
                if (existingTracker == null)
                {
                    _logger.LogWarning("Tracker {TrackerId} not found", id);
                    return ApiResponse<InvestmentTrackerResponse>.ErrorResponse(
                        $"Investment tracker with ID {id} not found",
                        404
                    );
                }

                // Cast to InvestmentTracker
                var tracker = existingTracker as InvestmentTracker;
                if (tracker == null)
                {
                    _logger.LogError("Failed to cast tracker {TrackerId} to InvestmentTracker", id);
                    return ApiResponse<InvestmentTrackerResponse>.ErrorResponse(
                        "Internal error: Invalid tracker type",
                        500
                    );
                }

                // Update status and notes
                tracker.Status = request.Status;
                if (!string.IsNullOrWhiteSpace(request.Notes))
                {
                    tracker.Notes = request.Notes;
                }

                await _trackerRepository.UpdateAsync(tracker);
                await _trackerRepository.SaveChangesAsync();

                // Retrieve updated tracker with navigation properties
                var updatedTracker = await _trackerRepository.GetByIdAsync(id);
                var response = _mapper.Map<InvestmentTrackerResponse>(updatedTracker);

                _logger.LogInformation("Tracker {TrackerId} status updated successfully", id);

                return ApiResponse<InvestmentTrackerResponse>.SuccessResponse(
                    response,
                    "Investment tracker status updated successfully",
                    200
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tracker {TrackerId} status", id);
                return ApiResponse<InvestmentTrackerResponse>.ErrorResponse(
                    "An error occurred while updating the investment tracker status",
                    500
                );
            }
        }

        /// <summary>
        /// Gets all trackers for dashboard view
        /// </summary>
        public async Task<ApiResponse<List<DashboardItemResponse>>> GetDashboardAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving dashboard items");

                var trackers = await _trackerRepository.GetDashboardItemsAsync();
                var dashboardItems = new List<DashboardItemResponse>();

                foreach (var tracker in trackers)
                {
                    var item = _mapper.Map<DashboardItemResponse>(tracker);
                    dashboardItems.Add(item);
                }

                _logger.LogInformation("Retrieved {Count} dashboard items", dashboardItems.Count);

                return ApiResponse<List<DashboardItemResponse>>.SuccessResponse(
                    dashboardItems,
                    "Dashboard items retrieved successfully",
                    200
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard items");
                return ApiResponse<List<DashboardItemResponse>>.ErrorResponse(
                    "An error occurred while retrieving dashboard items",
                    500
                );
            }
        }

        /// <summary>
        /// Gets a specific tracker by ID with full details
        /// </summary>
        public async Task<ApiResponse<InvestmentTrackerResponse>> GetTrackerByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving tracker {TrackerId}", id);

                var tracker = await _trackerRepository.GetByIdAsync(id);
                if (tracker == null)
                {
                    _logger.LogWarning("Tracker {TrackerId} not found", id);
                    return ApiResponse<InvestmentTrackerResponse>.ErrorResponse(
                        $"Investment tracker with ID {id} not found",
                        404
                    );
                }

                var response = _mapper.Map<InvestmentTrackerResponse>(tracker);

                _logger.LogInformation("Retrieved tracker {TrackerId}", id);

                return ApiResponse<InvestmentTrackerResponse>.SuccessResponse(
                    response,
                    "Investment tracker retrieved successfully",
                    200
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tracker {TrackerId}", id);
                return ApiResponse<InvestmentTrackerResponse>.ErrorResponse(
                    "An error occurred while retrieving the investment tracker",
                    500
                );
            }
        }

        /// <summary>
        /// Deletes an investment tracker
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteTrackerAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting tracker {TrackerId}", id);

                // Check if tracker exists
                var existingTracker = await _trackerRepository.GetByIdAsync(id);
                if (existingTracker == null)
                {
                    _logger.LogWarning("Tracker {TrackerId} not found", id);
                    return ApiResponse<bool>.ErrorResponse(
                        $"Investment tracker with ID {id} not found",
                        404
                    );
                }

                // Cast to InvestmentTracker
                var tracker = existingTracker as InvestmentTracker;
                if (tracker == null)
                {
                    _logger.LogError("Failed to cast tracker {TrackerId} to InvestmentTracker", id);
                    return ApiResponse<bool>.ErrorResponse(
                        "Internal error: Invalid tracker type",
                        500
                    );
                }

                await _trackerRepository.DeleteAsync(tracker);
                await _trackerRepository.SaveChangesAsync();

                _logger.LogInformation("Tracker {TrackerId} deleted successfully", id);

                return ApiResponse<bool>.SuccessResponse(
                    true,
                    "Investment tracker deleted successfully",
                    200
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tracker {TrackerId}", id);
                return ApiResponse<bool>.ErrorResponse(
                    "An error occurred while deleting the investment tracker",
                    500
                );
            }
        }
    }
}