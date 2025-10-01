using Microsoft.EntityFrameworkCore;
using TWS.Core.Enums;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.Beneficiaries;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.Beneficiaries
{
    /// <summary>
    /// Repository implementation for Beneficiary entity operations
    /// Extends GenericRepository with beneficiary-specific methods
    /// Reference: DatabaseSchema.md Table 19
    /// </summary>
    public class BeneficiaryRepository : GenericRepository<Beneficiary>, IBeneficiaryRepository
    {
        public BeneficiaryRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all beneficiaries for a specific investor profile
        /// Returns beneficiaries ordered by Type, then PercentageOfBenefit descending
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <returns>Collection of beneficiaries</returns>
        public async Task<IEnumerable<object>> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            try
            {
                return await _dbSet
                    .Where(b => b.InvestorProfileId == investorProfileId)
                    .OrderBy(b => b.BeneficiaryType)
                    .ThenByDescending(b => b.PercentageOfBenefit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving beneficiaries for investor profile {investorProfileId}", ex);
            }
        }

        /// <summary>
        /// Gets beneficiaries of a specific type for an investor profile
        /// Filters by both investor and beneficiary type (Primary or Contingent)
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <param name="beneficiaryType">The beneficiary type (1=Primary, 2=Contingent)</param>
        /// <returns>Collection of beneficiaries of the specified type</returns>
        public async Task<IEnumerable<object>> GetByTypeAsync(int investorProfileId, int beneficiaryType)
        {
            try
            {
                // Cast int to BeneficiaryType enum
                var type = (BeneficiaryType)beneficiaryType;

                return await _dbSet
                    .Where(b => b.InvestorProfileId == investorProfileId && b.BeneficiaryType == type)
                    .OrderByDescending(b => b.PercentageOfBenefit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving beneficiaries of type {beneficiaryType} for investor profile {investorProfileId}", ex);
            }
        }

        /// <summary>
        /// Calculates the total percentage allocated for a specific beneficiary type
        /// Used for validating that percentages total 100% per type
        /// Business logic validation performed in service layer
        /// </summary>
        /// <param name="investorProfileId">The investor profile ID</param>
        /// <param name="beneficiaryType">The beneficiary type (1=Primary, 2=Contingent)</param>
        /// <returns>Sum of percentages for the specified type</returns>
        public async Task<decimal> GetTotalPercentageByTypeAsync(int investorProfileId, int beneficiaryType)
        {
            try
            {
                // Cast int to BeneficiaryType enum
                var type = (BeneficiaryType)beneficiaryType;

                var total = await _dbSet
                    .Where(b => b.InvestorProfileId == investorProfileId && b.BeneficiaryType == type)
                    .SumAsync(b => b.PercentageOfBenefit);

                return total;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calculating total percentage for beneficiary type {beneficiaryType} for investor profile {investorProfileId}", ex);
            }
        }

        // Explicit implementations for interface methods
        async Task<object?> IBeneficiaryRepository.GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        async Task<IEnumerable<object>> IBeneficiaryRepository.GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        async Task<object> IBeneficiaryRepository.AddAsync(object entity)
        {
            return await base.AddAsync((Beneficiary)entity);
        }

        Task IBeneficiaryRepository.UpdateAsync(object entity)
        {
            return base.UpdateAsync((Beneficiary)entity);
        }

        Task IBeneficiaryRepository.DeleteAsync(object entity)
        {
            return base.DeleteAsync((Beneficiary)entity);
        }

        Task<bool> IBeneficiaryRepository.SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}