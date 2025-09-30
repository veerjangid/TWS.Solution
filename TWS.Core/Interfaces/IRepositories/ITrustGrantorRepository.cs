namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for TrustGrantor entity
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will use IGenericRepository.
    /// Reference: DatabaseSchema.md Table 15
    /// </summary>
    public interface ITrustGrantorRepository
    {
        // Inherits all CRUD methods from IGenericRepository<TrustGrantor> in implementation
    }
}