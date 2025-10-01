using TWS.Core.Enums;

namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for InvestmentExperience entity
    /// Provides specific operations for investment experiences
    /// Reference: DatabaseSchema.md Table 20
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will specify the entity type.
    /// </summary>
    public interface IInvestmentExperienceRepository
    {
        /// <summary>
        /// Retrieves all investment experiences for a primary investor
        /// </summary>
        /// <param name="primaryInvestorInfoId">Primary investor info ID</param>
        /// <returns>Collection of investment experiences</returns>
        Task<IEnumerable<object>> GetByPrimaryInvestorInfoIdAsync(int primaryInvestorInfoId);

        /// <summary>
        /// Retrieves investment experience for a specific asset class
        /// </summary>
        /// <param name="primaryInvestorInfoId">Primary investor info ID</param>
        /// <param name="assetClass">Asset class type</param>
        /// <returns>Investment experience or null if not found</returns>
        Task<object?> GetByAssetClassAsync(int primaryInvestorInfoId, AssetClass assetClass);

        /// <summary>
        /// Gets investment experience by ID
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all investment experiences
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Adds a new investment experience
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates an investment experience
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes an investment experience
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}