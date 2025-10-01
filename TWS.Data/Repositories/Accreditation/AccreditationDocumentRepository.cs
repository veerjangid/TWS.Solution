using Microsoft.EntityFrameworkCore;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.Accreditation;

namespace TWS.Data.Repositories.Accreditation
{
    /// <summary>
    /// Repository implementation for AccreditationDocument entity
    /// Provides data access operations for accreditation documents
    /// Reference: DatabaseSchema.md Table 24
    /// </summary>
    public class AccreditationDocumentRepository : IAccreditationDocumentRepository
    {
        private readonly TWSDbContext _context;

        public AccreditationDocumentRepository(TWSDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all documents for a specific accreditation record
        /// </summary>
        public async Task<IEnumerable<object>> GetByAccreditationIdAsync(int accreditationId)
        {
            return await _context.AccreditationDocuments
                .Where(ad => ad.InvestorAccreditationId == accreditationId)
                .ToListAsync();
        }

        /// <summary>
        /// Gets documents by document type for a specific accreditation
        /// </summary>
        public async Task<IEnumerable<object>> GetByTypeAsync(int accreditationId, string documentType)
        {
            return await _context.AccreditationDocuments
                .Where(ad => ad.InvestorAccreditationId == accreditationId && ad.DocumentType == documentType)
                .ToListAsync();
        }

        /// <summary>
        /// Gets document by ID
        /// </summary>
        public async Task<object?> GetByIdAsync(int id)
        {
            return await _context.AccreditationDocuments.FindAsync(id);
        }

        /// <summary>
        /// Gets all documents
        /// </summary>
        public async Task<IEnumerable<object>> GetAllAsync()
        {
            return await _context.AccreditationDocuments.ToListAsync();
        }

        /// <summary>
        /// Adds a new document
        /// </summary>
        public async Task<object> AddAsync(object entity)
        {
            var document = entity as AccreditationDocument;
            if (document == null)
                throw new ArgumentException("Invalid entity type", nameof(entity));

            await _context.AccreditationDocuments.AddAsync(document);
            return document;
        }

        /// <summary>
        /// Updates a document
        /// </summary>
        public async Task UpdateAsync(object entity)
        {
            var document = entity as AccreditationDocument;
            if (document == null)
                throw new ArgumentException("Invalid entity type", nameof(entity));

            document.UpdatedAt = DateTime.UtcNow;
            _context.AccreditationDocuments.Update(document);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Deletes a document
        /// </summary>
        public async Task DeleteAsync(object entity)
        {
            var document = entity as AccreditationDocument;
            if (document == null)
                throw new ArgumentException("Invalid entity type", nameof(entity));

            _context.AccreditationDocuments.Remove(document);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Saves changes to database
        /// </summary>
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}