using Microsoft.EntityFrameworkCore;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;
using TWS.Data.Entities.Accreditation;

namespace TWS.Data.Repositories.Accreditation
{
    /// <summary>
    /// Repository implementation for InvestorAccreditation entity
    /// Provides data access operations for investor accreditation
    /// Reference: DatabaseSchema.md Table 23
    /// </summary>
    public class InvestorAccreditationRepository : IInvestorAccreditationRepository
    {
        private readonly TWSDbContext _context;

        public InvestorAccreditationRepository(TWSDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets accreditation record by investor profile ID
        /// </summary>
        public async Task<object?> GetByInvestorProfileIdAsync(int investorProfileId)
        {
            return await _context.InvestorAccreditations
                .FirstOrDefaultAsync(ia => ia.InvestorProfileId == investorProfileId);
        }

        /// <summary>
        /// Gets accreditation record with all related documents
        /// </summary>
        public async Task<object?> GetWithDocumentsAsync(int id)
        {
            return await _context.InvestorAccreditations
                .Include(ia => ia.AccreditationDocuments)
                .FirstOrDefaultAsync(ia => ia.Id == id);
        }

        /// <summary>
        /// Gets accreditation record by investor profile ID with all related documents
        /// </summary>
        public async Task<object?> GetByInvestorProfileIdWithDocumentsAsync(int investorProfileId)
        {
            return await _context.InvestorAccreditations
                .Include(ia => ia.AccreditationDocuments)
                .FirstOrDefaultAsync(ia => ia.InvestorProfileId == investorProfileId);
        }

        /// <summary>
        /// Gets accreditation by ID
        /// </summary>
        public async Task<object?> GetByIdAsync(int id)
        {
            return await _context.InvestorAccreditations.FindAsync(id);
        }

        /// <summary>
        /// Gets all accreditation records
        /// </summary>
        public async Task<IEnumerable<object>> GetAllAsync()
        {
            return await _context.InvestorAccreditations.ToListAsync();
        }

        /// <summary>
        /// Adds a new accreditation record
        /// </summary>
        public async Task<object> AddAsync(object entity)
        {
            var accreditation = entity as InvestorAccreditation;
            if (accreditation == null)
                throw new ArgumentException("Invalid entity type", nameof(entity));

            await _context.InvestorAccreditations.AddAsync(accreditation);
            return accreditation;
        }

        /// <summary>
        /// Updates an accreditation record
        /// </summary>
        public async Task UpdateAsync(object entity)
        {
            var accreditation = entity as InvestorAccreditation;
            if (accreditation == null)
                throw new ArgumentException("Invalid entity type", nameof(entity));

            accreditation.UpdatedAt = DateTime.UtcNow;
            _context.InvestorAccreditations.Update(accreditation);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Deletes an accreditation record
        /// </summary>
        public async Task DeleteAsync(object entity)
        {
            var accreditation = entity as InvestorAccreditation;
            if (accreditation == null)
                throw new ArgumentException("Invalid entity type", nameof(entity));

            _context.InvestorAccreditations.Remove(accreditation);
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