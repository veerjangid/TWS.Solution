using Microsoft.EntityFrameworkCore;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.Core;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.Core
{
    /// <summary>
    /// Repository implementation for InvestorProfile entity
    /// Inherits from GenericRepository for common CRUD operations
    /// Implements custom query methods with eager loading of type-specific profiles
    /// Reference: DatabaseSchema.md Table 4, Architecture.md
    /// </summary>
    public class InvestorProfileRepository : GenericRepository<InvestorProfile>, IInvestorProfileRepository
    {
        public InvestorProfileRepository(TWSDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets an investor profile by UserId with all type-specific details loaded
        /// Includes all 5 type-specific profile navigations (Individual, Joint, IRA, Trust, Entity)
        /// </summary>
        /// <param name="userId">ApplicationUser ID</param>
        /// <returns>InvestorProfile if found, null otherwise</returns>
        public async Task<object?> GetByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId cannot be null or empty", nameof(userId));

            return await _context.InvestorProfiles
                .Include(ip => ip.User)
                .Include(ip => ip.IndividualProfile)
                .Include(ip => ip.JointProfile)
                .Include(ip => ip.IRAProfile)
                .Include(ip => ip.TrustProfile)
                .Include(ip => ip.EntityProfile)
                .FirstOrDefaultAsync(ip => ip.UserId == userId);
        }

        /// <summary>
        /// Gets an investor profile by ID with all type-specific details loaded
        /// Includes all 5 type-specific profile navigations based on InvestorType
        /// </summary>
        /// <param name="id">InvestorProfile ID</param>
        /// <returns>InvestorProfile with related details if found, null otherwise</returns>
        public async Task<object?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.InvestorProfiles
                .Include(ip => ip.User)
                .Include(ip => ip.IndividualProfile)
                .Include(ip => ip.JointProfile)
                .Include(ip => ip.IRAProfile)
                .Include(ip => ip.TrustProfile)
                .Include(ip => ip.EntityProfile)
                .FirstOrDefaultAsync(ip => ip.Id == id);
        }

        /// <summary>
        /// Checks if a user already has an investor profile
        /// </summary>
        /// <param name="userId">ApplicationUser ID</param>
        /// <returns>True if profile exists, false otherwise</returns>
        public async Task<bool> HasProfileAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId cannot be null or empty", nameof(userId));

            return await _context.InvestorProfiles
                .AnyAsync(ip => ip.UserId == userId);
        }

        // Explicit implementations for interface methods
        async Task<object?> IInvestorProfileRepository.GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        async Task<IEnumerable<object>> IInvestorProfileRepository.GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        async Task<object> IInvestorProfileRepository.AddAsync(object entity)
        {
            return await base.AddAsync((InvestorProfile)entity);
        }

        Task IInvestorProfileRepository.UpdateAsync(object entity)
        {
            return base.UpdateAsync((InvestorProfile)entity);
        }

        Task IInvestorProfileRepository.DeleteAsync(object entity)
        {
            return base.DeleteAsync((InvestorProfile)entity);
        }

        Task<bool> IInvestorProfileRepository.SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}