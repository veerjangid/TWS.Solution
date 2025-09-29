using Microsoft.EntityFrameworkCore;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.Core;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.Core
{
    /// <summary>
    /// Repository implementation for AccountRequest entity
    /// Inherits from GenericRepository for common CRUD operations
    /// Implements custom query methods for specific business needs
    /// Reference: DatabaseSchema.md Table 3, Architecture.md
    /// </summary>
    public class RequestAccountRepository : GenericRepository<AccountRequest>, IRequestAccountRepository
    {
        public RequestAccountRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all unprocessed account requests with ProcessedByUser included
        /// Orders by RequestDate descending (newest first)
        /// </summary>
        public async Task<IEnumerable<object>> GetUnprocessedRequestsAsync()
        {
            return await _context.AccountRequests
                .Where(ar => !ar.IsProcessed)
                .Include(ar => ar.ProcessedByUser)
                .OrderByDescending(ar => ar.RequestDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all processed account requests with ProcessedByUser included
        /// Orders by ProcessedDate descending (most recently processed first)
        /// </summary>
        public async Task<IEnumerable<object>> GetProcessedRequestsAsync()
        {
            return await _context.AccountRequests
                .Where(ar => ar.IsProcessed)
                .Include(ar => ar.ProcessedByUser)
                .OrderByDescending(ar => ar.ProcessedDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets an account request by email address
        /// Includes ProcessedByUser navigation property
        /// </summary>
        /// <param name="email">Email address to search for</param>
        /// <returns>AccountRequest if found, null otherwise</returns>
        public async Task<object?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty", nameof(email));

            return await _context.AccountRequests
                .Include(ar => ar.ProcessedByUser)
                .FirstOrDefaultAsync(ar => ar.Email.ToLower() == email.ToLower());
        }

        // Explicit implementations for interface methods
        async Task<object?> IRequestAccountRepository.GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        async Task<IEnumerable<object>> IRequestAccountRepository.GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        async Task<object> IRequestAccountRepository.AddAsync(object entity)
        {
            return await base.AddAsync((AccountRequest)entity);
        }

        Task IRequestAccountRepository.UpdateAsync(object entity)
        {
            return base.UpdateAsync((AccountRequest)entity);
        }

        Task IRequestAccountRepository.DeleteAsync(object entity)
        {
            return base.DeleteAsync((AccountRequest)entity);
        }

        Task<bool> IRequestAccountRepository.SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}