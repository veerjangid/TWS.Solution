using Microsoft.EntityFrameworkCore;
using TWS.Core.Enums;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.Core;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.Core
{
    /// <summary>
    /// Repository implementation for InvestorInvestment junction entity
    /// Inherits from GenericRepository for common CRUD operations
    /// Implements custom query methods with eager loading of related entities
    /// Reference: DatabaseSchema.md Table 31
    /// </summary>
    public class InvestorInvestmentRepository : GenericRepository<InvestorInvestment>, IInvestorInvestmentRepository
    {
        public InvestorInvestmentRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets an investment by ID with offering and investor profile details loaded
        /// </summary>
        /// <param name="id">InvestorInvestment ID</param>
        /// <returns>InvestorInvestment with related details if found, null otherwise</returns>
        public async Task<object?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.InvestorInvestments
                .Include(ii => ii.InvestorProfile)
                .Include(ii => ii.Offering)
                .FirstOrDefaultAsync(ii => ii.Id == id);
        }

        /// <summary>
        /// Gets all investments for a specific investor profile with offering details
        /// </summary>
        /// <param name="investorProfileId">InvestorProfile ID</param>
        /// <returns>Collection of investments for the investor</returns>
        public async Task<IEnumerable<object>> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            return await _context.InvestorInvestments
                .Include(ii => ii.Offering)
                .Where(ii => ii.InvestorProfileId == investorProfileId)
                .OrderByDescending(ii => ii.InvestmentDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all investments for a specific offering with investor profile details
        /// </summary>
        /// <param name="offeringId">Offering ID</param>
        /// <returns>Collection of investments for the offering</returns>
        public async Task<IEnumerable<object>> GetByOfferingIdAsync(int offeringId)
        {
            return await _context.InvestorInvestments
                .Include(ii => ii.InvestorProfile)
                .Where(ii => ii.OfferingId == offeringId)
                .OrderByDescending(ii => ii.InvestmentDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets investments by status
        /// </summary>
        /// <param name="status">Investment status</param>
        /// <returns>Collection of investments with specified status</returns>
        public async Task<IEnumerable<object>> GetByStatusAsync(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status cannot be null or empty", nameof(status));

            // Parse status string to enum
            if (!Enum.TryParse<InvestmentStatus>(status, true, out var investmentStatus))
                throw new ArgumentException($"Invalid investment status: {status}", nameof(status));

            return await _context.InvestorInvestments
                .Include(ii => ii.InvestorProfile)
                .Include(ii => ii.Offering)
                .Where(ii => ii.Status == investmentStatus)
                .OrderByDescending(ii => ii.InvestmentDate)
                .ToListAsync();
        }

        /// <summary>
        /// Checks if an investor has already invested in a specific offering
        /// Used to enforce unique constraint at application level
        /// </summary>
        /// <param name="investorProfileId">InvestorProfile ID</param>
        /// <param name="offeringId">Offering ID</param>
        /// <returns>True if investment exists, false otherwise</returns>
        public async Task<bool> HasInvestmentAsync(int investorProfileId, int offeringId)
        {
            return await _context.InvestorInvestments
                .AnyAsync(ii => ii.InvestorProfileId == investorProfileId && ii.OfferingId == offeringId);
        }

        // Explicit implementations for interface methods
        async Task<IEnumerable<object>> IInvestorInvestmentRepository.GetAllAsync()
        {
            return await _context.InvestorInvestments
                .Include(ii => ii.InvestorProfile)
                .Include(ii => ii.Offering)
                .OrderByDescending(ii => ii.InvestmentDate)
                .ToListAsync();
        }

        async Task<object?> IInvestorInvestmentRepository.GetByIdAsync(int id)
        {
            return await _context.InvestorInvestments
                .Include(ii => ii.Offering)
                .FirstOrDefaultAsync(ii => ii.Id == id);
        }

        async Task<object> IInvestorInvestmentRepository.AddAsync(object entity)
        {
            return await base.AddAsync((InvestorInvestment)entity);
        }

        Task IInvestorInvestmentRepository.UpdateAsync(object entity)
        {
            return base.UpdateAsync((InvestorInvestment)entity);
        }

        Task IInvestorInvestmentRepository.DeleteAsync(object entity)
        {
            return base.DeleteAsync((InvestorInvestment)entity);
        }

        Task<bool> IInvestorInvestmentRepository.SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}