namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for OfferingDocument entity with custom query methods
    /// Inherits from IGenericRepository for common CRUD operations
    /// </summary>
    public interface IOfferingDocumentRepository
    {
        /// <summary>
        /// Gets all offering documents
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Gets an offering document by ID
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all documents for a specific offering
        /// </summary>
        Task<IEnumerable<object>> GetByOfferingIdAsync(int offeringId);

        /// <summary>
        /// Adds a new offering document
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates an offering document
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes an offering document
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}