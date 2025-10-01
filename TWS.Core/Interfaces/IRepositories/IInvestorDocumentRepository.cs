namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for InvestorDocument entity operations
    /// Reference: DatabaseSchema.md Table 23
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will specify the entity type.
    /// </summary>
    public interface IInvestorDocumentRepository
    {
        /// <summary>
        /// Gets all documents for a specific investor profile
        /// Returns documents ordered by UploadDate descending (newest first)
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <returns>Collection of documents</returns>
        Task<IEnumerable<object>> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Gets a document by ID
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all documents
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Adds a new document
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Adds multiple documents
        /// </summary>
        Task<IEnumerable<object>> AddRangeAsync(IEnumerable<object> entities);

        /// <summary>
        /// Updates a document
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes a document
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Checks if a document exists by ID
        /// </summary>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}