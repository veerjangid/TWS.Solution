using Microsoft.EntityFrameworkCore;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.Portal;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.Portal
{
    /// <summary>
    /// Repository implementation for OfferingDocument entity
    /// Inherits from GenericRepository for common CRUD operations
    /// Implements custom query methods for offering document management
    /// </summary>
    public class OfferingDocumentRepository : GenericRepository<OfferingDocument>, IOfferingDocumentRepository
    {
        public OfferingDocumentRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all documents for a specific offering
        /// </summary>
        /// <param name="offeringId">ID of the offering</param>
        /// <returns>Collection of documents for the offering</returns>
        public async Task<IEnumerable<object>> GetByOfferingIdAsync(int offeringId)
        {
            return await _context.OfferingDocuments
                .Where(od => od.OfferingId == offeringId)
                .OrderByDescending(od => od.UploadDate)
                .ToListAsync();
        }

        // Explicit implementations for interface methods
        async Task<IEnumerable<object>> IOfferingDocumentRepository.GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        async Task<object?> IOfferingDocumentRepository.GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        async Task<object> IOfferingDocumentRepository.AddAsync(object entity)
        {
            return await base.AddAsync((OfferingDocument)entity);
        }

        Task IOfferingDocumentRepository.UpdateAsync(object entity)
        {
            return base.UpdateAsync((OfferingDocument)entity);
        }

        Task IOfferingDocumentRepository.DeleteAsync(object entity)
        {
            return base.DeleteAsync((OfferingDocument)entity);
        }

        Task<bool> IOfferingDocumentRepository.SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}