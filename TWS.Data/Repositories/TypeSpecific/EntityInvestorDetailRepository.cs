using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.TypeSpecific;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.TypeSpecific
{
    /// <summary>
    /// Repository implementation for EntityInvestorDetail entity
    /// Inherits from GenericRepository for common CRUD operations
    /// Reference: DatabaseSchema.md Table 9, Architecture.md
    /// </summary>
    public class EntityInvestorDetailRepository : GenericRepository<EntityInvestorDetail>, IEntityInvestorDetailRepository
    {
        public EntityInvestorDetailRepository(TWSDbContext context) : base(context)
        {
        }

        // All CRUD operations inherited from GenericRepository<EntityInvestorDetail>
        // No additional custom methods required at this time
    }
}