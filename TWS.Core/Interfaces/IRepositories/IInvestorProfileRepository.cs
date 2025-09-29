namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for InvestorProfile entity with custom query methods
    /// Inherits from IGenericRepository for common CRUD operations
    /// Reference: DatabaseSchema.md Table 4
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will specify the entity type.
    /// </summary>
    public interface IInvestorProfileRepository
    {
        // Inherits all CRUD methods from IGenericRepository<InvestorProfile> in implementation

        /// <summary>
        /// Gets an investor profile by UserId with all type-specific details loaded
        /// </summary>
        /// <param name="userId">ApplicationUser ID</param>
        /// <returns>InvestorProfile if found, null otherwise</returns>
        Task<object?> GetByUserIdAsync(string userId);

        /// <summary>
        /// Gets an investor profile by ID with all type-specific details loaded
        /// Includes Individual, Joint, IRA, Trust, and Entity profiles based on InvestorType
        /// </summary>
        /// <param name="id">InvestorProfile ID</param>
        /// <returns>InvestorProfile with related details if found, null otherwise</returns>
        Task<object?> GetByIdWithDetailsAsync(int id);

        /// <summary>
        /// Checks if a user already has an investor profile
        /// </summary>
        /// <param name="userId">ApplicationUser ID</param>
        /// <returns>True if profile exists, false otherwise</returns>
        Task<bool> HasProfileAsync(string userId);

        /// <summary>
        /// Gets all investor profiles with optional filtering by InvestorType
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Gets an investor profile by ID
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new investor profile
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates an investor profile
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes an investor profile
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}