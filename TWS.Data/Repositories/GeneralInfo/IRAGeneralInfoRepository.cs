using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.GeneralInfo;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.GeneralInfo
{
    /// <summary>
    /// Repository implementation for IRAGeneralInfo entity
    /// Reference: DatabaseSchema.md Table 13
    /// </summary>
    public class IRAGeneralInfoRepository : GenericRepository<IRAGeneralInfo>, IIRAGeneralInfoRepository
    {
        public IRAGeneralInfoRepository(TWSDbContext context) : base(context)
        {
        }
    }
}