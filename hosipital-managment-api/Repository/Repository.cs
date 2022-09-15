using hosipital_managment_api.Data;
using hosipital_managment_api.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace hosipital_managment_api.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal AppDbContext _context;
        internal DbSet<TEntity> _dbSet;
        public Repository(AppDbContext context,DbSet<TEntity> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
        }
        public virtual async Task<TEntity> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll(List<string>? includes = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.AsNoTracking().ToListAsync();
        }
        public virtual async Task<TEntity> FindOne(Expression<Func<TEntity, bool>> expression, List<string>? includes = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> expression, List<string>? includes = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (includes != null)
            {
                foreach(var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }
            
            return await query.AsNoTracking().ToListAsync();
        }
        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
             _dbSet.AddRange(entities);
        }
        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        public virtual async Task Delete(object id)
        {
            TEntity entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
        }
        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
