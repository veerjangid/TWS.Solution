using Microsoft.EntityFrameworkCore;
using TWS.Core.Enums;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.PrimaryInvestorInfo;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.PrimaryInvestorInfo
{
    /// <summary>
    /// Repository implementation for InvestmentExperience entity
    /// Provides data access operations for investment experiences
    /// Reference: DatabaseSchema.md Table 20
    /// </summary>
    public class InvestmentExperienceRepository : GenericRepository<InvestmentExperience>, IInvestmentExperienceRepository
    {
        public InvestmentExperienceRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves all investment experiences for a primary investor
        /// </summary>
        public async Task<IEnumerable<object>> GetByPrimaryInvestorInfoIdAsync(int primaryInvestorInfoId)
        {
            return await _context.InvestmentExperiences
                .Where(i => i.PrimaryInvestorInfoId == primaryInvestorInfoId)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves investment experience for a specific asset class
        /// </summary>
        public async Task<object?> GetByAssetClassAsync(int primaryInvestorInfoId, AssetClass assetClass)
        {
            return await _context.InvestmentExperiences
                .FirstOrDefaultAsync(i => i.PrimaryInvestorInfoId == primaryInvestorInfoId
                                       && i.AssetClass == assetClass);
        }

        // Explicit interface implementation for object-based methods
        async Task<object?> IInvestmentExperienceRepository.GetByIdAsync(int id) => await base.GetByIdAsync(id);
        async Task<IEnumerable<object>> IInvestmentExperienceRepository.GetAllAsync() => await base.GetAllAsync();
        async Task<object> IInvestmentExperienceRepository.AddAsync(object entity) => await base.AddAsync((InvestmentExperience)entity);
        Task IInvestmentExperienceRepository.UpdateAsync(object entity) => base.UpdateAsync((InvestmentExperience)entity);
        Task IInvestmentExperienceRepository.DeleteAsync(object entity) => base.DeleteAsync((InvestmentExperience)entity);
        Task<bool> IInvestmentExperienceRepository.SaveChangesAsync() => base.SaveChangesAsync();
    }
}