namespace Estately.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void AddAsync(TEntity entity);
        ValueTask<TEntity> GetByIdAsync(int id);
        ValueTask<IEnumerable<TEntity>> ReadAllAsync();
        ValueTask<IEnumerable<TEntity>> ReadAllIncluding(params string[] includes);
        ValueTask<IEnumerable<TEntity>> ReadWithPagination(int page, int pageSize);
        ValueTask<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate);
        void UpdateAsync(TEntity entity);
        void DeleteAsync(int id);
        int GetMaxId();
    }
}
