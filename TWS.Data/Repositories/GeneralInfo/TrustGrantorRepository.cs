using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.GeneralInfo;
using TWS.Data.Repositories.Base;

namespace TWS.Data.Repositories.GeneralInfo
{
    /// <summary>
    /// Repository implementation for TrustGrantor entity
    /// Reference: DatabaseSchema.md Table 15
    /// </summary>
    public class TrustGrantorRepository : GenericRepository<TrustGrantor>, ITrustGrantorRepository
    {
        public TrustGrantorRepository(TWSDbContext context) : base(context)
        {
        }
    }
}