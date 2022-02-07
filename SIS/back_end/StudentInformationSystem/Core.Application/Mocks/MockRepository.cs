using Core.Application.Interfaces;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Mocks
{
    public class MockRepository<T> : IRepository<T> where T : EntityBase
    {
        protected readonly List<T> entities;

        public MockRepository(IMockDataContext context)
        {
            this.entities = context.GetEntities<T>();
        }

        public virtual Task<T> UpdateAsync(Guid id, object input, CancellationToken cancellationToken = default)
        {
            int index = entities.FindIndex(g => g.Id == id);
            if (index == -1) return null;
            entities[index] = (T)input;
            return Task.FromResult((T)input); // this will break eventually!
        }

        public void Add(T item)
        {
            item.Id = Guid.NewGuid();
            entities.Add(item);
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => {
                int index = entities.FindIndex(e => e.Id == id);
                if (index == -1) return; // TODO: error handling, mediator should return Response object!
                entities.RemoveAt(index);
            }, cancellationToken
            );
        }

        public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(entities.AsEnumerable());
        }

        public Task<T> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(entities.Find(g => g.Id == id));
        }

        public IQueryable<T> Query()
        {
            return entities.AsQueryable();
        }

        Task<int> IRepository<T>.CommitAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IRepository<T>.DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Query(params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IContextAsync<T> ContextAsync(IQueryable<T> query)
        {
            throw new NotImplementedException();
        }
    }
}
