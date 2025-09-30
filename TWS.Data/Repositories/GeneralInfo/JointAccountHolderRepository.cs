using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.GeneralInfo;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.GeneralInfo
{
    /// <summary>
    /// Repository implementation for JointAccountHolder entity
    /// Reference: DatabaseSchema.md Table 12
    /// </summary>
    public class JointAccountHolderRepository : GenericRepository<JointAccountHolder>, IJointAccountHolderRepository
    {
        public JointAccountHolderRepository(TWSDbContext context) : base(context)
        {
        }
    }
}