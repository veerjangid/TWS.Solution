namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for Beneficiary entity operations
    /// Reference: DatabaseSchema.md Table 19
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will specify the entity type.
    /// </summary>
    public interface IBeneficiaryRepository
    {
        /// <summary>
        /// Gets all beneficiaries for a specific investor profile
        /// Returns beneficiaries ordered by Type, then PercentageOfBenefit descending
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <returns>Collection of beneficiaries</returns>
        Task<IEnumerable<object>> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Gets beneficiaries of a specific type for an investor profile
        /// Filters by both investor and beneficiary type (Primary or Contingent)
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <param name="beneficiaryType">The beneficiary type (1=Primary, 2=Contingent)</param>
        /// <returns>Collection of beneficiaries of the specified type</returns>
        Task<IEnumerable<object>> GetByTypeAsync(int investorProfileId, int beneficiaryType);

        /// <summary>
        /// Calculates the total percentage allocated for a specific beneficiary type
        /// Used for validating that percentages total 100% per type
        /// Business logic validation performed in service layer
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <param name="beneficiaryType">The beneficiary type (1=Primary, 2=Contingent)</param>
        /// <returns>Sum of percentages for the specified type</returns>
        Task<decimal> GetTotalPercentageByTypeAsync(int investorProfileId, int beneficiaryType);

        /// <summary>
        /// Gets a beneficiary by ID
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all beneficiaries
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Adds a new beneficiary
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates a beneficiary
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes a beneficiary
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}