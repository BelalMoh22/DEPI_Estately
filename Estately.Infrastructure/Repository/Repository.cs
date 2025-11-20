
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
        public async ValueTask<TEntity> GetByIdAsync(int? id)
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

        public async ValueTask<TEntity?> GetByIdIncludingAsync(int id, params string[] includes)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();

            foreach (var include in includes)
                query = query.Include(include);

            // Detect primary key name automatically
            var keyName = _context.Model
                .FindEntityType(typeof(TEntity))
                .FindPrimaryKey()
                .Properties
                .Select(x => x.Name)
                .First();

            return await query.FirstOrDefaultAsync(
                x => EF.Property<int>(x, keyName) == id
            );
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
        /*
         public async Task<(IEnumerable<TEntity> Data, int TotalCount)> 
    ReadPagedAsync(
        int page, 
        int pageSize, 
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params string[] includes)
{
    IQueryable<TEntity> query = _dbSet.AsQueryable();

    // Apply Includes
    foreach (var include in includes)
    {
        query = query.Include(include);
    }

    // Filter
    if (filter != null)
        query = query.Where(filter);

    // Total count AFTER filter
    int totalCount = await query.CountAsync();

    // Sorting
    if (orderBy != null)
        query = orderBy(query);

    // Pagination
    var data = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToListAsync();

    return (data, totalCount);
}
*/
        public async ValueTask<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<int> CounterAsync()
        {
            return await _dbSet.CountAsync();
        }

        public int GetMaxId()
        {
            var property = _context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties.Select(x => x.Name).Single();

            var maxId = _dbSet.AsNoTracking().Max(e => EF.Property<int>(e, property));

            return maxId;
        }

        public IQueryable<TEntity> Query()
        {
            return _context.Set<TEntity>().AsQueryable();
        } 

    public async Task DeletePropertyFeatureMappingAsync(int propertyId, int featureId)
        {
            var entity = await _context.TblPropertyFeaturesMappings
                .FirstOrDefaultAsync(x => x.PropertyID == propertyId && x.FeatureID == featureId);

            if (entity != null)
            {
                _context.TblPropertyFeaturesMappings.Remove(entity);
            }
        }
        #endregion
    }
}
