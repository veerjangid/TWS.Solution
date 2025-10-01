namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for InvestmentTracker entity with custom query methods
    /// Portal/CRM module for tracking investments with financial metrics
    /// Reference: DatabaseSchema.md Table 32
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will specify the entity type.
    /// </summary>
    public interface IInvestmentTrackerRepository
    {
        /// <summary>
        /// Gets all investment trackers
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Gets an investment tracker by ID with navigation properties loaded
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all trackers for dashboard view with navigation properties loaded
        /// Used for Portal CRM dashboard display
        /// </summary>
        Task<IEnumerable<object>> GetDashboardItemsAsync();

        /// <summary>
        /// Gets all trackers for a specific offering
        /// </summary>
        Task<IEnumerable<object>> GetByOfferingIdAsync(int offeringId);

        /// <summary>
        /// Gets all trackers for a specific investor profile
        /// </summary>
        Task<IEnumerable<object>> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Gets all trackers by investment status
        /// </summary>
        Task<IEnumerable<object>> GetByStatusAsync(string status);

        /// <summary>
        /// Gets all trackers by lead owner/licensed rep
        /// </summary>
        Task<IEnumerable<object>> GetByLeadOwnerAsync(string leadOwner);

        /// <summary>
        /// Gets all trackers by investment type
        /// </summary>
        Task<IEnumerable<object>> GetByInvestmentTypeAsync(string investmentType);

        /// <summary>
        /// Adds a new investment tracker
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates an existing investment tracker
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes an investment tracker
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Checks if tracker exists by ID
        /// </summary>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}