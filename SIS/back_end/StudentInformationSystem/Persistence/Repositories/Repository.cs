using Core.Application.Interfaces;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        protected readonly PersistenceContext _context;
        public Repository(PersistenceContext context)
        {
            _context = context;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public virtual async Task<T> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<T> UpdateAsync(Guid id, object input, CancellationToken cancellationToken = default)
        {
            if (input == null) return null;

            T exist = await GetAsync(id, cancellationToken);
            if (exist == null) return null;

            _context.Entry(exist).CurrentValues.SetValues(input);
            return exist;
            // NB: Don't forget to call Commit!
        }

        public void Add(T item)
        {
            _context.Set<T>().Add(item);
            // NB: Don't forget to call Commit!
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var item = await GetAsync(id, cancellationToken);
            _context.Set<T>().Remove(item);
            // NB: Don't forget to call Commit!
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public virtual IQueryable<T> Query() => _context.Set<T>();

        public IQueryable<T> Query(params Expression<Func<T, object>>[] includes)
        {
            IIncludableQueryable<T,object> qry = null;
            foreach(var include in includes)
            {
                qry = qry == null ? _context.Set<T>().Include(include) : qry.Include(include);
            }
            return qry;
        }

        public IContextAsync<T> ContextAsync(IQueryable<T> query)
        {
            return new ContextAsync<T>(query);
        }

  
    }
}
