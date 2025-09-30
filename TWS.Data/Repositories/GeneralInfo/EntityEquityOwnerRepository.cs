using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.GeneralInfo;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.GeneralInfo
{
    /// <summary>
    /// Repository implementation for EntityEquityOwner entity
    /// Reference: DatabaseSchema.md Table 17
    /// </summary>
    public class EntityEquityOwnerRepository : GenericRepository<EntityEquityOwner>, IEntityEquityOwnerRepository
    {
        public EntityEquityOwnerRepository(TWSDbContext context) : base(context)
        {
        }
    }
}