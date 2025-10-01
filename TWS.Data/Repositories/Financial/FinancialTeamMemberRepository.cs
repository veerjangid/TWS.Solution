using Microsoft.EntityFrameworkCore;
using TWS.Core.Enums;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.Financial;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.Financial
{
    /// <summary>
    /// Repository implementation for FinancialTeamMember entity.
    /// Extends GenericRepository with custom query methods.
    /// Reference: DatabaseSchema.md Table 29
    /// </summary>
    public class FinancialTeamMemberRepository : GenericRepository<FinancialTeamMember>, IFinancialTeamMemberRepository
    {
        public FinancialTeamMemberRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves all Financial Team Members for a specific investor profile.
        /// Returns members ordered by MemberType, then Name.
        /// Includes navigation property to InvestorProfile.
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile.</param>
        /// <returns>Collection of FinancialTeamMember records.</returns>
        public async Task<IEnumerable<object>> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            return await _dbSet
                .Include(ftm => ftm.InvestorProfile)
                .Where(ftm => ftm.InvestorProfileId == investorProfileId)
                .OrderBy(ftm => ftm.MemberType)
                .ThenBy(ftm => ftm.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Gets financial team members of a specific type for an investor profile.
        /// Filters by both investor and member type (Accountant, Attorney, etc.).
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <param name="memberType">The member type enum value (1-5)</param>
        /// <returns>Collection of financial team members of the specified type</returns>
        public async Task<IEnumerable<object>> GetByMemberTypeAsync(int investorProfileId, int memberType)
        {
            var memberTypeEnum = (FinancialTeamMemberType)memberType;

            return await _dbSet
                .Include(ftm => ftm.InvestorProfile)
                .Where(ftm => ftm.InvestorProfileId == investorProfileId && ftm.MemberType == memberTypeEnum)
                .OrderBy(ftm => ftm.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a Financial Team Member record by ID.
        /// Includes navigation property to InvestorProfile.
        /// </summary>
        public new async Task<object?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(ftm => ftm.InvestorProfile)
                .FirstOrDefaultAsync(ftm => ftm.Id == id);
        }

        /// <summary>
        /// Gets all Financial Team Member records.
        /// Includes navigation property to InvestorProfile.
        /// </summary>
        public new async Task<IEnumerable<object>> GetAllAsync()
        {
            return await _dbSet
                .Include(ftm => ftm.InvestorProfile)
                .OrderBy(ftm => ftm.InvestorProfileId)
                .ThenBy(ftm => ftm.MemberType)
                .ThenBy(ftm => ftm.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new Financial Team Member record.
        /// </summary>
        public async Task<object> AddAsync(object entity)
        {
            if (entity is not FinancialTeamMember ftm)
                throw new ArgumentException("Entity must be of type FinancialTeamMember", nameof(entity));

            await _dbSet.AddAsync(ftm);
            return ftm;
        }

        /// <summary>
        /// Updates a Financial Team Member record.
        /// </summary>
        public Task UpdateAsync(object entity)
        {
            if (entity is not FinancialTeamMember ftm)
                throw new ArgumentException("Entity must be of type FinancialTeamMember", nameof(entity));

            _dbSet.Update(ftm);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes a Financial Team Member record.
        /// </summary>
        public Task DeleteAsync(object entity)
        {
            if (entity is not FinancialTeamMember ftm)
                throw new ArgumentException("Entity must be of type FinancialTeamMember", nameof(entity));

            _dbSet.Remove(ftm);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Saves changes to database.
        /// </summary>
        public new async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}