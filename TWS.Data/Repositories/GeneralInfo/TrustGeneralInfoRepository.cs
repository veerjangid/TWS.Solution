using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.GeneralInfo;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.GeneralInfo
{
    /// <summary>
    /// Repository implementation for TrustGeneralInfo entity
    /// Reference: DatabaseSchema.md Table 14
    /// </summary>
    public class TrustGeneralInfoRepository : GenericRepository<TrustGeneralInfo>, ITrustGeneralInfoRepository
    {
        public TrustGeneralInfoRepository(TWSDbContext context) : base(context)
        {
        }
    }
}