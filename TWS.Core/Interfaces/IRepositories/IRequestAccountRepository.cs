namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for AccountRequest entity with custom query methods
    /// Inherits from IGenericRepository for common CRUD operations
    /// Reference: DatabaseSchema.md Table 3
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will specify the entity type.
    /// </summary>
    public interface IRequestAccountRepository
    {
        // Inherits all CRUD methods from IGenericRepository<AccountRequest> in implementation

        /// <summary>
        /// Gets all unprocessed account requests
        /// </summary>
        /// <returns>Collection of unprocessed requests</returns>
        Task<IEnumerable<object>> GetUnprocessedRequestsAsync();

        /// <summary>
        /// Gets all processed account requests
        /// </summary>
        /// <returns>Collection of processed requests</returns>
        Task<IEnumerable<object>> GetProcessedRequestsAsync();

        /// <summary>
        /// Gets an account request by email address
        /// </summary>
        /// <param name="email">Email address to search for</param>
        /// <returns>AccountRequest if found, null otherwise</returns>
        Task<object?> GetByEmailAsync(string email);

        /// <summary>
        /// Gets an account request by ID
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all account requests
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Adds a new account request
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates an account request
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes an account request
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}