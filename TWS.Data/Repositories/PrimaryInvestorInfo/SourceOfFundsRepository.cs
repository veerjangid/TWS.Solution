using Microsoft.EntityFrameworkCore;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.PrimaryInvestorInfo;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.PrimaryInvestorInfo
{
    /// <summary>
    /// Repository implementation for SourceOfFunds entity
    /// Provides data access operations for source of funds
    /// Reference: DatabaseSchema.md Table 21
    /// </summary>
    public class SourceOfFundsRepository : GenericRepository<SourceOfFunds>, ISourceOfFundsRepository
    {
        public SourceOfFundsRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves all source of funds for a primary investor
        /// </summary>
        public async Task<IEnumerable<object>> GetByPrimaryInvestorInfoIdAsync(int primaryInvestorInfoId)
        {
            return await _context.SourceOfFunds
                .Where(s => s.PrimaryInvestorInfoId == primaryInvestorInfoId)
                .ToListAsync();
        }

        // Explicit interface implementation for object-based methods
        async Task<object?> ISourceOfFundsRepository.GetByIdAsync(int id) => await base.GetByIdAsync(id);
        async Task<IEnumerable<object>> ISourceOfFundsRepository.GetAllAsync() => await base.GetAllAsync();
        async Task<object> ISourceOfFundsRepository.AddAsync(object entity) => await base.AddAsync((SourceOfFunds)entity);
        Task ISourceOfFundsRepository.UpdateAsync(object entity) => base.UpdateAsync((SourceOfFunds)entity);
        Task ISourceOfFundsRepository.DeleteAsync(object entity) => base.DeleteAsync((SourceOfFunds)entity);
        Task<bool> ISourceOfFundsRepository.SaveChangesAsync() => base.SaveChangesAsync();
    }
}