namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for PrimaryInvestorInfo entity
    /// Provides specific operations for primary investor info
    /// Reference: DatabaseSchema.md Table 18
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will specify the entity type.
    /// </summary>
    public interface IPrimaryInvestorInfoRepository
    {
        /// <summary>
        /// Retrieves primary investor info by investor profile ID
        /// </summary>
        /// <param name="investorProfileId">Investor profile ID</param>
        /// <returns>Primary investor info or null if not found</returns>
        Task<object?> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Retrieves primary investor info with all related entities (broker affiliations, experiences, etc.)
        /// </summary>
        /// <param name="id">Primary investor info ID</param>
        /// <returns>Primary investor info with related data or null if not found</returns>
        Task<object?> GetWithRelatedDataAsync(int id);

        /// <summary>
        /// Gets primary investor info by ID
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all primary investor info records
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Adds a new primary investor info record
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates a primary investor info record
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes a primary investor info record
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}