namespace Estately.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        ValueTask<TEntity> GetByIdAsync(int? id);
        ValueTask<IEnumerable<TEntity>> ReadAllAsync();
        ValueTask<IEnumerable<TEntity>> ReadAllIncluding(params string[] includes);
        ValueTask<IEnumerable<TEntity>> ReadWithPagination(int page, int pageSize);
        ValueTask<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate);
        ValueTask<TEntity?> GetByIdIncludingAsync(int id, params string[] includes);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task<int> CounterAsync();
        int GetMaxId();
        IQueryable<TEntity> Query();
    }
}
