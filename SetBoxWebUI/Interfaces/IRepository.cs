using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SetBoxWebUI.Interfaces
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        void Add(TEntity entity);
        Task<int> AddAsync(TEntity entity);
        void AddRange(IEnumerable<TEntity> entity);
        Task<int> AddRangeAsync(IEnumerable<TEntity> entity);
        void Delete(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
        void DeleteById(TKey id);
        Task<int> DeleteByIdAsync(TKey id);
        void DeleteRange(Expression<Func<TEntity, bool>> filter);
        void DeleteRange(IEnumerable<TEntity> entity);
        Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> filter);
        Task<int> DeleteRangeAsync(IEnumerable<TEntity> entity);
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> orderBy = null);
        Task<List<TEntity>> GetAsync();
        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> orderBy = null);
        TEntity GetById(TKey id);
        Task<TEntity> GetByIdAsync(TKey id);
        Task<KeyValuePair<int, List<TEntity>>> GetPagination(Expression<Func<TEntity, bool>> filter, int page = 1, int count = 10);
        Task<KeyValuePair<int, List<TEntity>>> GetPagination(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> orderBy, Expression<Func<TEntity, object>> orderByDescending, int page = 1, int count = 10);
        void Update(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        void UpdateById(TEntity entity, TKey id);
        Task<int> UpdateByIdAsync(TEntity entity, TKey id);
    }
}
