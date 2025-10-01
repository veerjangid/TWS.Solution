using Microsoft.EntityFrameworkCore;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.PrimaryInvestorInfo;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.PrimaryInvestorInfo
{
    /// <summary>
    /// Repository implementation for TaxRate entity
    /// Provides data access operations for tax rates
    /// Reference: DatabaseSchema.md Table 22
    /// </summary>
    public class TaxRateRepository : GenericRepository<TaxRate>, ITaxRateRepository
    {
        public TaxRateRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves all tax rates for a primary investor
        /// </summary>
        public async Task<IEnumerable<object>> GetByPrimaryInvestorInfoIdAsync(int primaryInvestorInfoId)
        {
            return await _context.TaxRates
                .Where(t => t.PrimaryInvestorInfoId == primaryInvestorInfoId)
                .ToListAsync();
        }

        // Explicit interface implementation for object-based methods
        async Task<object?> ITaxRateRepository.GetByIdAsync(int id) => await base.GetByIdAsync(id);
        async Task<IEnumerable<object>> ITaxRateRepository.GetAllAsync() => await base.GetAllAsync();
        async Task<object> ITaxRateRepository.AddAsync(object entity) => await base.AddAsync((TaxRate)entity);
        Task ITaxRateRepository.UpdateAsync(object entity) => base.UpdateAsync((TaxRate)entity);
        Task ITaxRateRepository.DeleteAsync(object entity) => base.DeleteAsync((TaxRate)entity);
        Task<bool> ITaxRateRepository.SaveChangesAsync() => base.SaveChangesAsync();
    }
}