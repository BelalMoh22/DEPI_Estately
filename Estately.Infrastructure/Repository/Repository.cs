using Estately.Core.Interfaces;
using Estately.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estately.Infrastructure.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDBContext _context;

        public Repository(AppDBContext context)
        {
            _context = context;
        }

        public ValueTask<TEntity> AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TEntity> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<TEntity>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<TEntity>> ReadAllIncluding(params string[] includes)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<TEntity>> ReadWithPagination(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public int GetMaxId()
        {
            throw new NotImplementedException();
        }
    }
}
