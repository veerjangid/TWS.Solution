using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.TypeSpecific;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.TypeSpecific
{
    /// <summary>
    /// Repository implementation for TrustInvestorDetail entity
    /// Inherits from GenericRepository for common CRUD operations
    /// Reference: DatabaseSchema.md Table 8, Architecture.md
    /// </summary>
    public class TrustInvestorDetailRepository : GenericRepository<TrustInvestorDetail>, ITrustInvestorDetailRepository
    {
        public TrustInvestorDetailRepository(TWSDbContext context) : base(context)
        {
        }

        // All CRUD operations inherited from GenericRepository<TrustInvestorDetail>
        // No additional custom methods required at this time
    }
}