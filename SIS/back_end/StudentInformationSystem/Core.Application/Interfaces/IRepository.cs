using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface IRepository<T>
    {
        public Task<T> GetAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        public IQueryable<T> Query();
        public IQueryable<T> Query(params Expression<Func<T, object>>[] includes);
        void Add(T item);
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(Guid id, object item, CancellationToken cancellationToken = default);
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        Task DisposeAsync();
        public IContextAsync<T> ContextAsync(IQueryable<T> query);
    }
}
