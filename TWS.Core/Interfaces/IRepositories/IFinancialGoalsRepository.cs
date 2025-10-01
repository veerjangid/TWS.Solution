namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for FinancialGoals entity operations.
    /// Provides data access methods for investor financial goals and objectives.
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will specify the entity type.
    /// Reference: DatabaseSchema.md Table 27
    /// </summary>
    public interface IFinancialGoalsRepository
    {
        /// <summary>
        /// Retrieves the Financial Goals for a specific investor profile.
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile.</param>
        /// <returns>The FinancialGoals record if found, otherwise null.</returns>
        Task<object?> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Gets a Financial Goals record by ID.
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all Financial Goals records.
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Adds a new Financial Goals record.
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates a Financial Goals record.
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes a Financial Goals record.
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database.
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}