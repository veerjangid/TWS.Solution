namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for PersonalFinancialStatement entity operations.
    /// Provides data access methods for Personal Financial Statement documents.
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will specify the entity type.
    /// Reference: DatabaseSchema.md Table 26
    /// </summary>
    public interface IPersonalFinancialStatementRepository
    {
        /// <summary>
        /// Retrieves the Personal Financial Statement for a specific investor profile.
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile.</param>
        /// <returns>The PersonalFinancialStatement record if found, otherwise null.</returns>
        Task<object?> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Gets a Personal Financial Statement by ID.
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all Personal Financial Statements.
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Adds a new Personal Financial Statement.
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates a Personal Financial Statement.
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes a Personal Financial Statement.
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database.
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}