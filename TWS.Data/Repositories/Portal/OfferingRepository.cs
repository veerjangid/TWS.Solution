using Microsoft.EntityFrameworkCore;
using TWS.Core.Enums;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.Portal;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.Portal
{
    /// <summary>
    /// Repository implementation for Offering entity
    /// Inherits from GenericRepository for common CRUD operations
    /// Implements custom query methods for offering management
    /// Reference: DatabaseSchema.md Table 30
    /// </summary>
    public class OfferingRepository : GenericRepository<Offering>, IOfferingRepository
    {
        public OfferingRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all active offerings where status is "Raising"
        /// </summary>
        /// <returns>Collection of active offerings</returns>
        public async Task<IEnumerable<object>> GetActiveOfferingsAsync()
        {
            return await _context.Offerings
                .Where(o => o.Status == OfferingStatus.Raising)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets offerings by status
        /// </summary>
        /// <param name="status">Offering status (Raising, Closed, ComingSoon)</param>
        /// <returns>Collection of offerings with specified status</returns>
        public async Task<IEnumerable<object>> GetByStatusAsync(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status cannot be null or empty", nameof(status));

            // Parse status string to enum
            if (!Enum.TryParse<OfferingStatus>(status, true, out var offeringStatus))
                throw new ArgumentException($"Invalid offering status: {status}", nameof(status));

            return await _context.Offerings
                .Where(o => o.Status == offeringStatus)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();
        }

        // Explicit implementations for interface methods
        async Task<IEnumerable<object>> IOfferingRepository.GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        async Task<object?> IOfferingRepository.GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        async Task<object> IOfferingRepository.AddAsync(object entity)
        {
            return await base.AddAsync((Offering)entity);
        }

        Task IOfferingRepository.UpdateAsync(object entity)
        {
            return base.UpdateAsync((Offering)entity);
        }

        Task IOfferingRepository.DeleteAsync(object entity)
        {
            return base.DeleteAsync((Offering)entity);
        }

        Task<bool> IOfferingRepository.SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}