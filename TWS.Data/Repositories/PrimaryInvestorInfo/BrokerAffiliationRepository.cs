using Microsoft.EntityFrameworkCore;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.PrimaryInvestorInfo;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.PrimaryInvestorInfo
{
    /// <summary>
    /// Repository implementation for BrokerAffiliation entity
    /// Provides data access operations for broker affiliations
    /// Reference: DatabaseSchema.md Table 19
    /// </summary>
    public class BrokerAffiliationRepository : GenericRepository<BrokerAffiliation>, IBrokerAffiliationRepository
    {
        public BrokerAffiliationRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves all broker affiliations for a primary investor
        /// </summary>
        public async Task<IEnumerable<object>> GetByPrimaryInvestorInfoIdAsync(int primaryInvestorInfoId)
        {
            return await _context.BrokerAffiliations
                .Where(b => b.PrimaryInvestorInfoId == primaryInvestorInfoId)
                .ToListAsync();
        }

        // Explicit interface implementation for object-based methods
        async Task<object?> IBrokerAffiliationRepository.GetByIdAsync(int id) => await base.GetByIdAsync(id);
        async Task<IEnumerable<object>> IBrokerAffiliationRepository.GetAllAsync() => await base.GetAllAsync();
        async Task<object> IBrokerAffiliationRepository.AddAsync(object entity) => await base.AddAsync((BrokerAffiliation)entity);
        Task IBrokerAffiliationRepository.UpdateAsync(object entity) => base.UpdateAsync((BrokerAffiliation)entity);
        Task IBrokerAffiliationRepository.DeleteAsync(object entity) => base.DeleteAsync((BrokerAffiliation)entity);
        Task<bool> IBrokerAffiliationRepository.SaveChangesAsync() => base.SaveChangesAsync();
    }
}