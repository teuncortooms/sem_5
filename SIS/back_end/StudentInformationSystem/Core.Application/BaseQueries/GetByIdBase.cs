using AutoMapper;
using Core.Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.QueryUtil
{
    public abstract class GetByIdBase<TResult, TEntity> where TResult : class
    {
        public class Query : IRequest<TResult>
        {
            public Guid Id { get; }

            public Query(Guid id) { Id = id; }
        }


        public class HandlerBase : IRequestHandler<Query, TResult>
        {
            private readonly IRepository<TEntity> repository;
            private readonly IMapper mapper;

            public HandlerBase(IRepository<TEntity> repository, IMapper mapper)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
                this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<TResult> Handle(Query query, CancellationToken cancellationToken = default)
            {
                TEntity entity = await repository.GetAsync(query.Id, cancellationToken);
                if (entity == null) return null;

                var result = mapper.Map<TResult>(entity);

                return result;
            }
        }
    }
}