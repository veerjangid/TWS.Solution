using Microsoft.EntityFrameworkCore;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.PrimaryInvestorInfo;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.PrimaryInvestorInfo
{
    /// <summary>
    /// Repository implementation for PrimaryInvestorInfo entity
    /// Provides data access operations for primary investor info
    /// Reference: DatabaseSchema.md Table 18
    /// </summary>
    public class PrimaryInvestorInfoRepository : GenericRepository<Entities.PrimaryInvestorInfo.PrimaryInvestorInfo>, IPrimaryInvestorInfoRepository
    {
        public PrimaryInvestorInfoRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves primary investor info by investor profile ID
        /// </summary>
        public async Task<object?> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            return await _context.PrimaryInvestorInfos
                .FirstOrDefaultAsync(p => p.InvestorProfileId == investorProfileId);
        }

        /// <summary>
        /// Retrieves primary investor info with all related entities
        /// </summary>
        public async Task<object?> GetWithRelatedDataAsync(int id)
        {
            return await _context.PrimaryInvestorInfos
                .Include(p => p.InvestorProfile)
                .Include(p => p.BrokerAffiliations)
                .Include(p => p.InvestmentExperiences)
                .Include(p => p.SourceOfFunds)
                .Include(p => p.TaxRates)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Explicit interface implementation for object-based methods
        async Task<object?> IPrimaryInvestorInfoRepository.GetByIdAsync(int id) => await base.GetByIdAsync(id);
        async Task<IEnumerable<object>> IPrimaryInvestorInfoRepository.GetAllAsync() => await base.GetAllAsync();
        async Task<object> IPrimaryInvestorInfoRepository.AddAsync(object entity) => await base.AddAsync((Entities.PrimaryInvestorInfo.PrimaryInvestorInfo)entity);
        Task IPrimaryInvestorInfoRepository.UpdateAsync(object entity) => base.UpdateAsync((Entities.PrimaryInvestorInfo.PrimaryInvestorInfo)entity);
        Task IPrimaryInvestorInfoRepository.DeleteAsync(object entity) => base.DeleteAsync((Entities.PrimaryInvestorInfo.PrimaryInvestorInfo)entity);
        Task<bool> IPrimaryInvestorInfoRepository.SaveChangesAsync() => base.SaveChangesAsync();
    }
}