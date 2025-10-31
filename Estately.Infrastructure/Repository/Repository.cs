
using Estately.Infrastructure.Data;

namespace Estately.Infrastructure.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDBContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(AppDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        #region Methods
        public async ValueTask<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async ValueTask<IEnumerable<TEntity>> ReadAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async ValueTask<IEnumerable<TEntity>> ReadAllIncluding(params string[] includes)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async ValueTask<IEnumerable<TEntity>> ReadWithPagination(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var pagedList = _dbSet.AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            return await pagedList.ToListAsync();
        }

        public async ValueTask<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).AsNoTracking().ToListAsync();
        }

        public void AddAsync(TEntity entity)
        {
            _dbSet.AddAsync(entity);
        }
        public void UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void DeleteAsync(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public int GetMaxId()
        {
            var property = _context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties.Select(x => x.Name).Single();

            var maxId = _dbSet.AsNoTracking().Max(e => EF.Property<int>(e, property));

            return maxId;
        }
    } 
    #endregion
}
