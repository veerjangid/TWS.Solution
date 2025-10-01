namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for BrokerAffiliation entity
    /// Provides specific operations for broker affiliations
    /// Reference: DatabaseSchema.md Table 19
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will specify the entity type.
    /// </summary>
    public interface IBrokerAffiliationRepository
    {
        /// <summary>
        /// Retrieves all broker affiliations for a primary investor
        /// </summary>
        /// <param name="primaryInvestorInfoId">Primary investor info ID</param>
        /// <returns>Collection of broker affiliations</returns>
        Task<IEnumerable<object>> GetByPrimaryInvestorInfoIdAsync(int primaryInvestorInfoId);

        /// <summary>
        /// Gets broker affiliation by ID
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all broker affiliations
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Adds a new broker affiliation
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates a broker affiliation
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes a broker affiliation
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}