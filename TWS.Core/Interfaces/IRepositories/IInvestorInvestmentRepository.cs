namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for InvestorInvestment junction entity with custom query methods
    /// Inherits from IGenericRepository for common CRUD operations
    /// Reference: DatabaseSchema.md Table 31
    /// </summary>
    public interface IInvestorInvestmentRepository
    {
        /// <summary>
        /// Gets all investments
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Gets an investment by ID with related offering details
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets an investment by ID with offering and investor profile details loaded
        /// </summary>
        Task<object?> GetByIdWithDetailsAsync(int id);

        /// <summary>
        /// Gets all investments for a specific investor profile
        /// Includes offering details
        /// </summary>
        Task<IEnumerable<object>> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Gets all investments for a specific offering
        /// Includes investor profile details
        /// </summary>
        Task<IEnumerable<object>> GetByOfferingIdAsync(int offeringId);

        /// <summary>
        /// Gets investments by status
        /// </summary>
        Task<IEnumerable<object>> GetByStatusAsync(string status);

        /// <summary>
        /// Checks if an investor has already invested in a specific offering
        /// </summary>
        Task<bool> HasInvestmentAsync(int investorProfileId, int offeringId);

        /// <summary>
        /// Adds a new investment
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates an investment
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes an investment
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}