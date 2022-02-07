using AutoMapper;
using Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.Queries
{
    public static class GetPermissions
    {
        public class Query : IRequest<PagedListWrapper<AppClaimModel>>
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
            public string Filter { get; set; }
            public string OrderBy { get; set; }
        }

        public class Handler : IRequestHandler<Query, PagedListWrapper<AppClaimModel>>
        {
            private readonly IMapper mapper;

            public Handler(IMapper mapper)
            {
                this.mapper = mapper;
            }
            public async Task<PagedListWrapper<AppClaimModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                var permissions = new List<Claim>();

                foreach (var claim in UserClaims.Permissions)
                    permissions.Add(UserClaims.Claim(claim));

                permissions.Insert(0, UserClaims.Claim(UserClaims.All));

                var results = new List<Claim>();
                var claimQuery = permissions.AsQueryable().FilterQuery(request);
                var claimTotal = claimQuery.Count();

                foreach (var claim in claimQuery.PageQuery(request).ToList())
                {
                    results.Add(claim);
                }


                var result = mapper.ProjectTo<AppClaimModel>(results.AsQueryable()).ToList();

                return new PagedListWrapper<AppClaimModel> { TotalCount = permissions.Count, Results = result };
            }
        }

        private static IQueryable<Claim> PageQuery(this IQueryable<Claim> query, Query request)
        {
            return query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
        }

        private static IQueryable<Claim> FilterQuery(this IQueryable<Claim> query, Query request)
        {
            if (string.IsNullOrEmpty(request.Filter))
                return query;

            var filter = request.Filter.ToUpper();
            return query.Where(g =>
                g.Value.ToUpper().Contains(filter)
                );
        }

    }
}
