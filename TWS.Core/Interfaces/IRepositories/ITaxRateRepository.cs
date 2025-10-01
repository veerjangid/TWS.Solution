namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for TaxRate entity
    /// Provides specific operations for tax rates
    /// Reference: DatabaseSchema.md Table 22
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will specify the entity type.
    /// </summary>
    public interface ITaxRateRepository
    {
        /// <summary>
        /// Retrieves all tax rates for a primary investor
        /// </summary>
        /// <param name="primaryInvestorInfoId">Primary investor info ID</param>
        /// <returns>Collection of tax rates</returns>
        Task<IEnumerable<object>> GetByPrimaryInvestorInfoIdAsync(int primaryInvestorInfoId);

        /// <summary>
        /// Gets tax rate by ID
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all tax rates
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Adds a new tax rate
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates a tax rate
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes a tax rate
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}