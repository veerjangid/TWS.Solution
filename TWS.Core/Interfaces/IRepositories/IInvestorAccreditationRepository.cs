namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for InvestorAccreditation entity
    /// Provides data access methods for investor accreditation operations
    /// Reference: DatabaseSchema.md Table 23
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will specify the entity type.
    /// </summary>
    public interface IInvestorAccreditationRepository
    {
        /// <summary>
        /// Gets accreditation record by investor profile ID
        /// </summary>
        /// <param name="investorProfileId">Investor profile ID</param>
        /// <returns>InvestorAccreditation or null if not found</returns>
        Task<object?> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Gets accreditation record with all related documents
        /// </summary>
        /// <param name="id">Accreditation ID</param>
        /// <returns>InvestorAccreditation with documents or null if not found</returns>
        Task<object?> GetWithDocumentsAsync(int id);

        /// <summary>
        /// Gets accreditation record by investor profile ID with all related documents
        /// </summary>
        /// <param name="investorProfileId">Investor profile ID</param>
        /// <returns>InvestorAccreditation with documents or null if not found</returns>
        Task<object?> GetByInvestorProfileIdWithDocumentsAsync(int investorProfileId);

        /// <summary>
        /// Gets accreditation by ID
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all accreditation records
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Adds a new accreditation record
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates an accreditation record
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes an accreditation record
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}