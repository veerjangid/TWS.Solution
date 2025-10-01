using Microsoft.EntityFrameworkCore;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.Documents;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.Documents
{
    /// <summary>
    /// Repository implementation for InvestorDocument entity operations
    /// Extends GenericRepository with document-specific methods
    /// Reference: DatabaseSchema.md Table 23
    /// </summary>
    public class InvestorDocumentRepository : GenericRepository<InvestorDocument>, IInvestorDocumentRepository
    {
        public InvestorDocumentRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all documents for a specific investor profile
        /// Returns documents ordered by UploadDate descending (newest first)
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <returns>Collection of documents</returns>
        public async Task<IEnumerable<object>> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            try
            {
                return await _dbSet
                    .Where(d => d.InvestorProfileId == investorProfileId)
                    .OrderByDescending(d => d.UploadDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving documents for investor profile {investorProfileId}", ex);
            }
        }

        /// <summary>
        /// Checks if a document exists by ID
        /// </summary>
        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _dbSet.AnyAsync(d => d.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking if document {id} exists", ex);
            }
        }

        // Explicit implementations for interface methods
        async Task<object?> IInvestorDocumentRepository.GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        async Task<IEnumerable<object>> IInvestorDocumentRepository.GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        async Task<object> IInvestorDocumentRepository.AddAsync(object entity)
        {
            return await base.AddAsync((InvestorDocument)entity);
        }

        async Task<IEnumerable<object>> IInvestorDocumentRepository.AddRangeAsync(IEnumerable<object> entities)
        {
            var documents = entities.Cast<InvestorDocument>().ToList();
            return await base.AddRangeAsync(documents);
        }

        Task IInvestorDocumentRepository.UpdateAsync(object entity)
        {
            return base.UpdateAsync((InvestorDocument)entity);
        }

        Task IInvestorDocumentRepository.DeleteAsync(object entity)
        {
            return base.DeleteAsync((InvestorDocument)entity);
        }

        Task<bool> IInvestorDocumentRepository.SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}