using hosipital_managment_api.Extensions;
using hosipital_managment_api.Models;
using System.Linq.Expressions;

namespace hosipital_managment_api.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(int id);
        Task<PagedList<TEntity>> GetAll(PagingParameters parameters, List<string>? includes = null);

        Task<TEntity> FindOne(Expression<Func<TEntity, bool>> expression, List<string>? includess = null);
        Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> expression, List<string>? includes=null);
        Task<IEnumerable<TEntity>> FindPaged(PagingParameters parameters, Expression<Func<TEntity, bool>> expression,
            List<string>? includes = null);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        Task Delete(object id);
        void Delete (TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
    }
}
