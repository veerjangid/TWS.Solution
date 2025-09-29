using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.TypeSpecific;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.TypeSpecific
{
    /// <summary>
    /// Repository implementation for IndividualInvestorDetail entity
    /// Inherits from GenericRepository for common CRUD operations
    /// Reference: DatabaseSchema.md Table 5, Architecture.md
    /// </summary>
    public class IndividualInvestorDetailRepository : GenericRepository<IndividualInvestorDetail>, IIndividualInvestorDetailRepository
    {
        public IndividualInvestorDetailRepository(TWSDbContext context) : base(context)
        {
        }

        // All CRUD operations inherited from GenericRepository<IndividualInvestorDetail>
        // No additional custom methods required at this time
    }
}