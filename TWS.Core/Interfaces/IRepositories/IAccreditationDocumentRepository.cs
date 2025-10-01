namespace TWS.Core.Interfaces.IRepositories
{
    /// <summary>
    /// Repository interface for AccreditationDocument entity
    /// Provides data access methods for accreditation document operations
    /// Reference: DatabaseSchema.md Table 24
    /// Note: This is a marker interface. Concrete implementation in TWS.Data will specify the entity type.
    /// </summary>
    public interface IAccreditationDocumentRepository
    {
        /// <summary>
        /// Gets all documents for a specific accreditation record
        /// </summary>
        /// <param name="accreditationId">Accreditation ID</param>
        /// <returns>List of accreditation documents</returns>
        Task<IEnumerable<object>> GetByAccreditationIdAsync(int accreditationId);

        /// <summary>
        /// Gets documents by document type for a specific accreditation
        /// </summary>
        /// <param name="accreditationId">Accreditation ID</param>
        /// <param name="documentType">Document type</param>
        /// <returns>List of accreditation documents</returns>
        Task<IEnumerable<object>> GetByTypeAsync(int accreditationId, string documentType);

        /// <summary>
        /// Gets document by ID
        /// </summary>
        Task<object?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all documents
        /// </summary>
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// Adds a new document
        /// </summary>
        Task<object> AddAsync(object entity);

        /// <summary>
        /// Updates a document
        /// </summary>
        Task UpdateAsync(object entity);

        /// <summary>
        /// Deletes a document
        /// </summary>
        Task DeleteAsync(object entity);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}