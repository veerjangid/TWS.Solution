using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.GeneralInfo;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.GeneralInfo
{
    /// <summary>
    /// Repository implementation for IndividualGeneralInfo entity
    /// Reference: DatabaseSchema.md Table 10
    /// </summary>
    public class IndividualGeneralInfoRepository : GenericRepository<IndividualGeneralInfo>, IIndividualGeneralInfoRepository
    {
        public IndividualGeneralInfoRepository(TWSDbContext context) : base(context)
        {
        }
    }
}