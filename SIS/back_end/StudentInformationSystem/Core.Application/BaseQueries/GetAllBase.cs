using AutoMapper;
using Core.Application.Interfaces;
using LinqKit;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using Core.Domain.Entities;

namespace Core.Application.QueryUtil
{
    public abstract class GetAllBase<TResult, TEntity>
    {
        public class Query : IRequest<PagedListWrapper<TResult>>
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
            public string Filter { get; set; }
            public string OrderBy { get; set; }
        }

        public abstract class HandlerBase : IRequestHandler<Query, PagedListWrapper<TResult>>
        {
            protected readonly IRepository<TEntity> repository;
            private readonly IMapper mapper;

            protected HandlerBase(IRepository<TEntity> repository, IMapper mapper)
            {
                this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
                this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public Task<PagedListWrapper<TResult>> Handle(Query request, CancellationToken cancellationToken = default)
            {
                var entities = GetEntitiesAsQueryable();
                entities = FilterOnPermissions(entities);
                entities = Filter(entities, request.Filter);
                entities = entities.ApplySort(request.OrderBy);

                var pagedList = ToPagedList(entities, request.PageSize, request.Page, MapToModels);

                return Task.FromResult(pagedList);
            }

            protected virtual IQueryable<TEntity> GetEntitiesAsQueryable()
            {
                return repository.Query();
            }

            private IQueryable<TResult> MapToModels(IQueryable<TEntity> queryable)
            {
                return mapper.ProjectTo<TResult>(queryable);
            }

            private IQueryable<TEntity> Filter(IQueryable<TEntity> entities, string filter)
            {
                if (string.IsNullOrEmpty(filter)) return entities;

                var condition = FilterExpression(filter);

                return entities.Where(condition);
            }

            protected abstract ExpressionStarter<TEntity> FilterExpression(string filter);

            protected virtual IQueryable<TEntity> FilterOnPermissions(IQueryable<TEntity> entities)
            {
                return entities; // optional override
            }

            private static PagedListWrapper<TResult> ToPagedList(IQueryable<TEntity> entities, int pageSize, int page, Projection mapToModels)
            {
                if (entities == null) 
                    return new PagedListWrapper<TResult> { TotalCount = 0, Results = null }; ;
                int totalRecords = entities.Count();
                var entitiesPage = entities.Skip((page - 1) * pageSize).Take(pageSize);
                var requestModels = mapToModels(entitiesPage);

                return new PagedListWrapper<TResult> { TotalCount = totalRecords, Results = requestModels.ToList() };
            }

            private delegate IQueryable<TResult> Projection(IQueryable<TEntity> entities);

        }
    }
}