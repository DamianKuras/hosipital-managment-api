using System.Linq.Expressions;

namespace hosipital_managment_api.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(int id, List<string>? includes = null);
        Task<IEnumerable<TEntity>> GetAll(List<string>? includes = null);

        Task<TEntity> FindOne(Expression<Func<TEntity, bool>> expression, List<string>? includess = null);
        Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> expression, List<string>? includes=null);
        Task Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        Task Delete(object id);
        void Delete (TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
    }
}
