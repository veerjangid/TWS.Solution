namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for FinancialTeamMember entity operations
    /// Reference: DatabaseSchema.md Table 29
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will specify the entity type.
    /// </summary>
    public interface IFinancialTeamMemberRepository
    {
        /// <summary>
        /// Gets all financial team members for a specific investor profile
        /// Returns members ordered by MemberType, then Name
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <returns>Collection of financial team members</returns>
        Task<IEnumerable<object>> GetByInvestorProfileIdAsync(int investorProfileId);

        /// <summary>
        /// Gets financial team members of a specific type for an investor profile
        /// Filters by both investor and member type (Accountant, Attorney, etc.)
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <param name="memberType">The member type enum value</param>
        /// <returns>Collection of financial team members of the specified type</returns>
        Task<IEnumerable<object>> GetByMemberTypeAsync(int investorProfileId, int memberType);

        /// <summary>
        /// Gets a financial team member by ID
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all financial team members
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Adds a new financial team member
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates a financial team member
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes a financial team member
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}