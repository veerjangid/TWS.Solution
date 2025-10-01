namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for SourceOfFunds entity
    /// Provides specific operations for source of funds
    /// Reference: DatabaseSchema.md Table 21
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will specify the entity type.
    /// </summary>
    public interface ISourceOfFundsRepository
    {
        /// <summary>
        /// Retrieves all source of funds for a primary investor
        /// </summary>
        /// <param name="primaryInvestorInfoId">Primary investor info ID</param>
        /// <returns>Collection of source of funds</returns>
        Task<IEnumerable<object>> GetByPrimaryInvestorInfoIdAsync(int primaryInvestorInfoId);

        /// <summary>
        /// Gets source of funds by ID
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all source of funds
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Adds a new source of funds
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates a source of funds
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes a source of funds
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}