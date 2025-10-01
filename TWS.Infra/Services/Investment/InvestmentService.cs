using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TWS.Core.DTOs.Request.Investment;
using TWS.Core.DTOs.Response.Investment;
using TWS.Core.Enums;
using TWS.Core.Interfaces.IServices;
using TWS.Data.Context;
using TWS.Data.Entities.Core;

namespace TWS.Infra.Services.Investment
{
    /// <summary>
    /// Service implementation for Investment management
    /// Handles business logic for investor investments in offerings
    /// Reference: APIDoc.md Section 12
    /// </summary>
    public class InvestmentService : IInvestmentService
    {
        private readonly TWSDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<InvestmentService> _logger;

        public InvestmentService(
            TWSDbContext context,
            IMapper mapper,
            ILogger<InvestmentService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new investment
        /// Validates investor profile and offering exist
        /// Prevents duplicate investments in same offering
        /// </summary>
        public async Task<InvestmentResponse> CreateInvestmentAsync(CreateInvestmentRequest request)
        {
            try
            {
                _logger.LogInformation("Creating investment for InvestorProfile {InvestorProfileId} in Offering {OfferingId}",
                    request.InvestorProfileId, request.OfferingId);

                // Validate InvestorProfile exists
                var profileExists = await _context.InvestorProfiles
                    .AnyAsync(p => p.Id == request.InvestorProfileId);

                if (!profileExists)
                {
                    _logger.LogWarning("InvestorProfile {InvestorProfileId} not found", request.InvestorProfileId);
                    throw new InvalidOperationException($"Investor profile with ID {request.InvestorProfileId} not found");
                }

                // Validate Offering exists
                var offering = await _context.Offerings
                    .FirstOrDefaultAsync(o => o.Id == request.OfferingId);

                if (offering == null)
                {
                    _logger.LogWarning("Offering {OfferingId} not found", request.OfferingId);
                    throw new InvalidOperationException($"Offering with ID {request.OfferingId} not found");
                }

                // Check for duplicate investment
                var existingInvestment = await _context.InvestorInvestments
                    .AnyAsync(ii => ii.InvestorProfileId == request.InvestorProfileId &&
                                   ii.OfferingId == request.OfferingId);

                if (existingInvestment)
                {
                    _logger.LogWarning("Duplicate investment attempted: InvestorProfile {InvestorProfileId} already invested in Offering {OfferingId}",
                        request.InvestorProfileId, request.OfferingId);
                    throw new InvalidOperationException($"Investor has already invested in offering {offering.Name}");
                }

                // Create investment
                var investment = new InvestorInvestment
                {
                    InvestorProfileId = request.InvestorProfileId,
                    OfferingId = request.OfferingId,
                    Amount = request.Amount,
                    Notes = request.Notes,
                    InvestmentDate = DateTime.UtcNow,
                    Status = InvestmentStatus.NeedDSTToComeOut, // Default initial status
                    CreatedAt = DateTime.UtcNow
                };

                _context.InvestorInvestments.Add(investment);
                await _context.SaveChangesAsync();

                // Reload with offering details
                var createdInvestment = await _context.InvestorInvestments
                    .Include(ii => ii.Offering)
                    .FirstOrDefaultAsync(ii => ii.Id == investment.Id);

                var response = _mapper.Map<InvestmentResponse>(createdInvestment);

                _logger.LogInformation("Investment {InvestmentId} created successfully", investment.Id);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating investment");
                throw;
            }
        }

        /// <summary>
        /// Updates investment status
        /// Validates status value and investment existence
        /// </summary>
        public async Task<InvestmentResponse> UpdateStatusAsync(int investmentId, UpdateInvestmentStatusRequest request)
        {
            try
            {
                _logger.LogInformation("Updating status for Investment {InvestmentId} to {Status}",
                    investmentId, request.Status);

                // Validate status
                if (!Enum.TryParse<InvestmentStatus>(request.Status, true, out var newStatus))
                {
                    _logger.LogWarning("Invalid investment status: {Status}", request.Status);
                    throw new ArgumentException($"Invalid investment status: {request.Status}");
                }

                // Get investment
                var investment = await _context.InvestorInvestments
                    .Include(ii => ii.Offering)
                    .FirstOrDefaultAsync(ii => ii.Id == investmentId);

                if (investment == null)
                {
                    _logger.LogWarning("Investment {InvestmentId} not found", investmentId);
                    throw new InvalidOperationException($"Investment with ID {investmentId} not found");
                }

                // Update status
                investment.Status = newStatus;
                investment.UpdatedAt = DateTime.UtcNow;

                // Append notes if provided
                if (!string.IsNullOrWhiteSpace(request.Notes))
                {
                    investment.Notes = string.IsNullOrWhiteSpace(investment.Notes)
                        ? request.Notes
                        : $"{investment.Notes}\n{DateTime.UtcNow:yyyy-MM-dd HH:mm}: {request.Notes}";
                }

                await _context.SaveChangesAsync();

                var response = _mapper.Map<InvestmentResponse>(investment);

                _logger.LogInformation("Investment {InvestmentId} status updated to {Status}", investmentId, newStatus);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating investment status for Investment {InvestmentId}", investmentId);
                throw;
            }
        }

        /// <summary>
        /// Gets all investments for a specific investor profile
        /// </summary>
        public async Task<IEnumerable<InvestmentResponse>> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            try
            {
                _logger.LogInformation("Retrieving investments for InvestorProfile {InvestorProfileId}", investorProfileId);

                var investments = await _context.InvestorInvestments
                    .Include(ii => ii.Offering)
                    .Where(ii => ii.InvestorProfileId == investorProfileId)
                    .OrderByDescending(ii => ii.InvestmentDate)
                    .ToListAsync();

                var response = _mapper.Map<IEnumerable<InvestmentResponse>>(investments);

                _logger.LogInformation("Retrieved {Count} investments for InvestorProfile {InvestorProfileId}",
                    investments.Count, investorProfileId);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving investments for InvestorProfile {InvestorProfileId}", investorProfileId);
                throw;
            }
        }

        /// <summary>
        /// Gets detailed information about an investment
        /// Includes offering details
        /// </summary>
        public async Task<InvestmentDetailsResponse?> GetInvestmentDetailsAsync(int investmentId)
        {
            try
            {
                _logger.LogInformation("Retrieving investment details for Investment {InvestmentId}", investmentId);

                var investment = await _context.InvestorInvestments
                    .Include(ii => ii.Offering)
                    .Include(ii => ii.InvestorProfile)
                    .FirstOrDefaultAsync(ii => ii.Id == investmentId);

                if (investment == null)
                {
                    _logger.LogWarning("Investment {InvestmentId} not found", investmentId);
                    return null;
                }

                var response = _mapper.Map<InvestmentDetailsResponse>(investment);

                _logger.LogInformation("Retrieved investment details for Investment {InvestmentId}", investmentId);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving investment details for Investment {InvestmentId}", investmentId);
                throw;
            }
        }
    }
}