namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for Offering entity with custom query methods
    /// Inherits from IGenericRepository for common CRUD operations
    /// Reference: DatabaseSchema.md Table 30
    /// </summary>
    public interface IOfferingRepository
    {
        /// <summary>
        /// Gets all offerings
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Gets an offering by ID
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all active offerings (status = Raising)
        /// </summary>
        Task<IEnumerable<object>> GetActiveOfferingsAsync();

        /// <summary>
        /// Gets offerings by status
        /// </summary>
        Task<IEnumerable<object>> GetByStatusAsync(string status);

        /// <summary>
        /// Adds a new offering
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates an offering
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes an offering
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}