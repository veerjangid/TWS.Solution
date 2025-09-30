using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.GeneralInfo;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.GeneralInfo
{
    /// <summary>
    /// Repository implementation for EntityGeneralInfo entity
    /// Reference: DatabaseSchema.md Table 16
    /// </summary>
    public class EntityGeneralInfoRepository : GenericRepository<EntityGeneralInfo>, IEntityGeneralInfoRepository
    {
        public EntityGeneralInfoRepository(TWSDbContext context) : base(context)
        {
        }
    }
}