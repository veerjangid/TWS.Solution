using Microsoft.EntityFrameworkCore;
using TWS.Core.Enums;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.Portal;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.Portal
{
    /// <summary>
    /// Repository implementation for InvestmentTracker entity
    /// Inherits from GenericRepository for common CRUD operations
    /// Implements custom query methods for Portal/CRM investment tracking
    /// Reference: DatabaseSchema.md Table 32
    /// </summary>
    public class InvestmentTrackerRepository : GenericRepository<InvestmentTracker>, IInvestmentTrackerRepository
    {
        public InvestmentTrackerRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all investment trackers with navigation properties loaded
        /// </summary>
        private async Task<IEnumerable<InvestmentTracker>> GetAllTrackersAsync()
        {
            return await _context.InvestmentTrackers
                .Include(it => it.Offering)
                .Include(it => it.InvestorProfile)
                .OrderByDescending(it => it.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets an investment tracker by ID with navigation properties loaded
        /// </summary>
        private async Task<InvestmentTracker?> GetTrackerByIdAsync(int id)
        {
            return await _context.InvestmentTrackers
                .Include(it => it.Offering)
                .Include(it => it.InvestorProfile)
                .FirstOrDefaultAsync(it => it.Id == id);
        }

        /// <summary>
        /// Gets all trackers for dashboard view with navigation properties loaded
        /// Used for Portal CRM dashboard display
        /// </summary>
        private async Task<IEnumerable<InvestmentTracker>> GetDashboardTrackers()
        {
            return await _context.InvestmentTrackers
                .Include(it => it.Offering)
                .Include(it => it.InvestorProfile)
                .OrderByDescending(it => it.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all trackers for a specific offering
        /// </summary>
        private async Task<IEnumerable<InvestmentTracker>> GetTrackersByOfferingIdAsync(int offeringId)
        {
            return await _context.InvestmentTrackers
                .Include(it => it.Offering)
                .Include(it => it.InvestorProfile)
                .Where(it => it.OfferingId == offeringId)
                .OrderByDescending(it => it.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all trackers for a specific investor profile
        /// </summary>
        private async Task<IEnumerable<InvestmentTracker>> GetTrackersByInvestorProfileIdAsync(int investorProfileId)
        {
            return await _context.InvestmentTrackers
                .Include(it => it.Offering)
                .Include(it => it.InvestorProfile)
                .Where(it => it.InvestorProfileId == investorProfileId)
                .OrderByDescending(it => it.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all trackers by investment status
        /// </summary>
        private async Task<IEnumerable<InvestmentTracker>> GetTrackersByStatusAsync(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status cannot be null or empty", nameof(status));

            // Parse status string to enum
            if (!Enum.TryParse<InvestmentStatus>(status, true, out var investmentStatus))
                throw new ArgumentException($"Invalid investment status: {status}", nameof(status));

            return await _context.InvestmentTrackers
                .Include(it => it.Offering)
                .Include(it => it.InvestorProfile)
                .Where(it => it.Status == investmentStatus)
                .OrderByDescending(it => it.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all trackers by lead owner/licensed rep
        /// </summary>
        private async Task<IEnumerable<InvestmentTracker>> GetTrackersByLeadOwnerAsync(string leadOwner)
        {
            if (string.IsNullOrWhiteSpace(leadOwner))
                throw new ArgumentException("Lead owner cannot be null or empty", nameof(leadOwner));

            return await _context.InvestmentTrackers
                .Include(it => it.Offering)
                .Include(it => it.InvestorProfile)
                .Where(it => it.LeadOwnerLicensedRep.Contains(leadOwner))
                .OrderByDescending(it => it.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all trackers by investment type
        /// </summary>
        private async Task<IEnumerable<InvestmentTracker>> GetTrackersByInvestmentTypeAsync(string investmentType)
        {
            if (string.IsNullOrWhiteSpace(investmentType))
                throw new ArgumentException("Investment type cannot be null or empty", nameof(investmentType));

            // Parse investment type string to enum
            if (!Enum.TryParse<PortalInvestmentType>(investmentType, true, out var portalInvestmentType))
                throw new ArgumentException($"Invalid investment type: {investmentType}", nameof(investmentType));

            return await _context.InvestmentTrackers
                .Include(it => it.Offering)
                .Include(it => it.InvestorProfile)
                .Where(it => it.InvestmentType == portalInvestmentType)
                .OrderByDescending(it => it.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new investment tracker
        /// </summary>
        private async Task<InvestmentTracker> AddTrackerAsync(InvestmentTracker entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _context.InvestmentTrackers.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Updates an existing investment tracker
        /// </summary>
        private Task UpdateTrackerAsync(InvestmentTracker entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.InvestmentTrackers.Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes an investment tracker
        /// </summary>
        private Task DeleteTrackerAsync(InvestmentTracker entity)
        {
            _context.InvestmentTrackers.Remove(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Checks if tracker exists by ID
        /// </summary>
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.InvestmentTrackers.AnyAsync(it => it.Id == id);
        }

        // Explicit interface implementations
        async Task<IEnumerable<object>> IInvestmentTrackerRepository.GetAllAsync()
        {
            return await GetAllTrackersAsync();
        }

        async Task<object?> IInvestmentTrackerRepository.GetByIdAsync(int id)
        {
            return await GetTrackerByIdAsync(id);
        }

        async Task<IEnumerable<object>> IInvestmentTrackerRepository.GetDashboardItemsAsync()
        {
            return await GetDashboardTrackers();
        }

        async Task<IEnumerable<object>> IInvestmentTrackerRepository.GetByOfferingIdAsync(int offeringId)
        {
            return await GetTrackersByOfferingIdAsync(offeringId);
        }

        async Task<IEnumerable<object>> IInvestmentTrackerRepository.GetByInvestorProfileIdAsync(int investorProfileId)
        {
            return await GetTrackersByInvestorProfileIdAsync(investorProfileId);
        }

        async Task<IEnumerable<object>> IInvestmentTrackerRepository.GetByStatusAsync(string status)
        {
            return await GetTrackersByStatusAsync(status);
        }

        async Task<IEnumerable<object>> IInvestmentTrackerRepository.GetByLeadOwnerAsync(string leadOwner)
        {
            return await GetTrackersByLeadOwnerAsync(leadOwner);
        }

        async Task<IEnumerable<object>> IInvestmentTrackerRepository.GetByInvestmentTypeAsync(string investmentType)
        {
            return await GetTrackersByInvestmentTypeAsync(investmentType);
        }

        async Task<object> IInvestmentTrackerRepository.AddAsync(object entity)
        {
            return await AddTrackerAsync((InvestmentTracker)entity);
        }

        Task IInvestmentTrackerRepository.UpdateAsync(object entity)
        {
            return UpdateTrackerAsync((InvestmentTracker)entity);
        }

        Task IInvestmentTrackerRepository.DeleteAsync(object entity)
        {
            return DeleteTrackerAsync((InvestmentTracker)entity);
        }

        Task<bool> IInvestmentTrackerRepository.SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}