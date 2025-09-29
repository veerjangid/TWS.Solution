using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.TypeSpecific;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.TypeSpecific
{
    /// <summary>
    /// Repository implementation for JointInvestorDetail entity
    /// Inherits from GenericRepository for common CRUD operations
    /// Reference: DatabaseSchema.md Table 6, Architecture.md
    /// </summary>
    public class JointInvestorDetailRepository : GenericRepository<JointInvestorDetail>, IJointInvestorDetailRepository
    {
        public JointInvestorDetailRepository(TWSDbContext context) : base(context)
        {
        }

        // All CRUD operations inherited from GenericRepository<JointInvestorDetail>
        // No additional custom methods required at this time
    }
}