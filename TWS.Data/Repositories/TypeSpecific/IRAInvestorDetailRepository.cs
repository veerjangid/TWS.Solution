using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.TypeSpecific;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.TypeSpecific
{
    /// <summary>
    /// Repository implementation for IRAInvestorDetail entity
    /// Inherits from GenericRepository for common CRUD operations
    /// Reference: DatabaseSchema.md Table 7, Architecture.md
    /// CRITICAL: Handles 5 IRA types only
    /// </summary>
    public class IRAInvestorDetailRepository : GenericRepository<IRAInvestorDetail>, IIRAInvestorDetailRepository
    {
        public IRAInvestorDetailRepository(TWSDbContext context) : base(context)
        {
        }

        // All CRUD operations inherited from GenericRepository<IRAInvestorDetail>
        // No additional custom methods required at this time
    }
}