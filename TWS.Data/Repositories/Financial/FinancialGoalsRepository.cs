using Microsoft.EntityFrameworkCore;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.Financial;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.Financial
{
    /// <summary>
    /// Repository implementation for FinancialGoals entity.
    /// Extends GenericRepository with custom query methods.
    /// Reference: DatabaseSchema.md Table 27
    /// </summary>
    public class FinancialGoalsRepository : GenericRepository<FinancialGoals>, IFinancialGoalsRepository
    {
        public FinancialGoalsRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves the Financial Goals for a specific investor profile.
        /// Includes navigation property to InvestorProfile.
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile.</param>
        /// <returns>The FinancialGoals record if found, otherwise null.</returns>
        public async Task<object?> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            return await _dbSet
                .Include(fg => fg.InvestorProfile)
                .FirstOrDefaultAsync(fg => fg.InvestorProfileId == investorProfileId);
        }

        /// <summary>
        /// Gets a Financial Goals record by ID.
        /// </summary>
        public new async Task<object?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(fg => fg.InvestorProfile)
                .FirstOrDefaultAsync(fg => fg.Id == id);
        }

        /// <summary>
        /// Gets all Financial Goals records.
        /// </summary>
        public new async Task<IEnumerable<object>> GetAllAsync()
        {
            return await _dbSet
                .Include(fg => fg.InvestorProfile)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new Financial Goals record.
        /// </summary>
        public async Task<object> AddAsync(object entity)
        {
            if (entity is not FinancialGoals fg)
                throw new ArgumentException("Entity must be of type FinancialGoals", nameof(entity));

            await _dbSet.AddAsync(fg);
            return fg;
        }

        /// <summary>
        /// Updates a Financial Goals record.
        /// </summary>
        public Task UpdateAsync(object entity)
        {
            if (entity is not FinancialGoals fg)
                throw new ArgumentException("Entity must be of type FinancialGoals", nameof(entity));

            _dbSet.Update(fg);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes a Financial Goals record.
        /// </summary>
        public Task DeleteAsync(object entity)
        {
            if (entity is not FinancialGoals fg)
                throw new ArgumentException("Entity must be of type FinancialGoals", nameof(entity));

            _dbSet.Remove(fg);
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