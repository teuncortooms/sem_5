using AutoMapper;
using Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.Queries
{
    public static class GetRoles
    {
        public class Query : IRequest<PagedListWrapper<AppRoleModel>>
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
            public string Filter { get; set; }
            public string OrderBy { get; set; }
        }

        public class Handler : IRequestHandler<Query, PagedListWrapper<AppRoleModel>>
        {
            private readonly IMapper mapper;
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly IMediator mediator;

            public Handler(IMapper mapper, RoleManager<IdentityRole> roleManager, IMediator mediator)
            {
                this.mapper = mapper;
                this.roleManager = roleManager;
                this.mediator = mediator;
            }

            public async Task<PagedListWrapper<AppRoleModel>> Handle(Query request, CancellationToken cancellationToken)
            {

                var results = new List<AppRoleModel>();
                var roleQuery = roleManager.Roles.FilterQuery(request);
                var roleTotal = await roleQuery.CountAsync(cancellationToken).ConfigureAwait(false);

                foreach (var roleId in await roleQuery.PageQuery(request).Select(r=>r.Id).ToListAsync(cancellationToken).ConfigureAwait(false))
                {
                    results.Add(await mediator.Send(new GetRole.Query { Id = roleId }).ConfigureAwait(false));
                }

                return new PagedListWrapper<AppRoleModel> { TotalCount = roleTotal, Results = results };
            }

            
        }

        private static IQueryable<IdentityRole> PageQuery(this IQueryable<IdentityRole> query, Query request)
        {
            return query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
        }

        private static IQueryable<IdentityRole> FilterQuery(this IQueryable<IdentityRole> query, Query request)
        {
            if (string.IsNullOrEmpty(request.Filter))
                return query;

            var filter = request.Filter.ToUpper();
            return query.Where(g =>
                g.Name.ToUpper().Contains(filter)
                );
        }
    }
}
