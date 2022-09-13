using System.Linq.Expressions;

namespace hosipital_managment_api.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(int id, List<string>? includes);
        Task<IEnumerable<TEntity>> GetAll(List<string>? includes);

        Task<TEntity> FindOne(Expression<Func<TEntity, bool>> expression, List<string>? includes);
        Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> expression, List<string>? includes);
        Task Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        Task Delete(object id);
        Task Delete (TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
    }
}
