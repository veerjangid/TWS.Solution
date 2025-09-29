using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TWS.Core.Interfaces.IRepositories;
using TWS.Data.Context;

namespace TWS.Data.Repositories.Base
{
    /// <summary>
    /// Generic repository implementation for common CRUD operations
    /// Implements IGenericRepository interface with async patterns
    /// Reference: Architecture.md - Repository Pattern
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly TWSDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(TWSDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Gets an entity by integer ID
        /// </summary>
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Gets an entity by string ID (for Guid/Identity keys)
        /// </summary>
        public async Task<T?> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Gets all entities
        /// </summary>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Finds entities matching a predicate
        /// </summary>
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Finds first entity matching a predicate
        /// </summary>
        public async Task<T?> FindFirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Counts entities matching an optional predicate
        /// </summary>
        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null
                ? await _dbSet.CountAsync()
                : await _dbSet.CountAsync(predicate);
        }

        /// <summary>
        /// Checks if any entity matches the predicate
        /// </summary>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        /// <summary>
        /// Adds a new entity
        /// </summary>
        public async Task<T> AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _dbSet.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Adds multiple entities
        /// </summary>
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            var entityList = entities.ToList();
            await _dbSet.AddRangeAsync(entityList);
            return entityList;
        }

        /// <summary>
        /// Updates an entity
        /// </summary>
        public Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates multiple entities
        /// </summary>
        public Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _dbSet.UpdateRange(entities);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes an entity
        /// </summary>
        public Task DeleteAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes multiple entities
        /// </summary>
        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _dbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Saves changes to the database
        /// </summary>
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}