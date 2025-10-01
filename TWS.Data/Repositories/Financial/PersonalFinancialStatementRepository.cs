using Microsoft.EntityFrameworkCore;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.Financial;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.Financial
{
    /// <summary>
    /// Repository implementation for PersonalFinancialStatement entity.
    /// Extends GenericRepository with custom query methods.
    /// Reference: DatabaseSchema.md Table 26
    /// </summary>
    public class PersonalFinancialStatementRepository : GenericRepository<PersonalFinancialStatement>, IPersonalFinancialStatementRepository
    {
        public PersonalFinancialStatementRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves the Personal Financial Statement for a specific investor profile.
        /// Includes navigation property to InvestorProfile.
        /// </summary>
        /// <param name="investorProfileId">The ID of the investor profile.</param>
        /// <returns>The PersonalFinancialStatement record if found, otherwise null.</returns>
        public async Task<object?> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            return await _dbSet
                .Include(pfs => pfs.InvestorProfile)
                .FirstOrDefaultAsync(pfs => pfs.InvestorProfileId == investorProfileId);
        }

        /// <summary>
        /// Gets a Personal Financial Statement by ID.
        /// </summary>
        public new async Task<object?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(pfs => pfs.InvestorProfile)
                .FirstOrDefaultAsync(pfs => pfs.Id == id);
        }

        /// <summary>
        /// Gets all Personal Financial Statements.
        /// </summary>
        public new async Task<IEnumerable<object>> GetAllAsync()
        {
            return await _dbSet
                .Include(pfs => pfs.InvestorProfile)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new Personal Financial Statement.
        /// </summary>
        public async Task<object> AddAsync(object entity)
        {
            if (entity is not PersonalFinancialStatement pfs)
                throw new ArgumentException("Entity must be of type PersonalFinancialStatement", nameof(entity));

            await _dbSet.AddAsync(pfs);
            return pfs;
        }

        /// <summary>
        /// Updates a Personal Financial Statement.
        /// </summary>
        public Task UpdateAsync(object entity)
        {
            if (entity is not PersonalFinancialStatement pfs)
                throw new ArgumentException("Entity must be of type PersonalFinancialStatement", nameof(entity));

            _dbSet.Update(pfs);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes a Personal Financial Statement.
        /// </summary>
        public Task DeleteAsync(object entity)
        {
            if (entity is not PersonalFinancialStatement pfs)
                throw new ArgumentException("Entity must be of type PersonalFinancialStatement", nameof(entity));

            _dbSet.Remove(pfs);
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