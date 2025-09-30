using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.GeneralInfo;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.GeneralInfo
{
    /// <summary>
    /// Repository implementation for JointGeneralInfo entity
    /// Reference: DatabaseSchema.md Table 11
    /// </summary>
    public class JointGeneralInfoRepository : GenericRepository<JointGeneralInfo>, IJointGeneralInfoRepository
    {
        public JointGeneralInfoRepository(TWSDbContext context) : base(context)
        {
        }
    }
}