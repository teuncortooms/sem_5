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
    public static class GetUsers
    {
        public class Query : IRequest<PagedListWrapper<AppUserModel>>
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
            public string Filter { get; set; }
            public string OrderBy { get; set; }
        }

        public class Handler : IRequestHandler<Query, PagedListWrapper<AppUserModel>>
        {
            private readonly IMapper mapper;
            private readonly UserManager<AppUser> userManager;
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly IMediator mediator;

            public Handler(IMapper mapper, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMediator mediator)
            {
                this.mapper = mapper;
                this.userManager = userManager;
                this.roleManager = roleManager;
                this.mediator = mediator;
            }

            public async Task<PagedListWrapper<AppUserModel>> Handle(Query request, CancellationToken cancellationToken)
            {

                var results = new List<AppUserModel>();

                var usrQuery = userManager.Users.FilterQuery(request);
                var usrTotal = await usrQuery.CountAsync(cancellationToken).ConfigureAwait(false);
               
                foreach(var usr in await usrQuery.PageQuery(request).ToListAsync(cancellationToken).ConfigureAwait(false))
                {
                    var transform = await mediator.Send(new GetUser.Query { Id = usr.Id }, cancellationToken).ConfigureAwait(false);
                    results.Add(transform);
                }

                return new PagedListWrapper<AppUserModel> { TotalCount = usrTotal, Results = results };
            }



        }

        private static IQueryable<AppUser> PageQuery(this IQueryable<AppUser> query, Query request)
        {
            return query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
        }

        private static IQueryable<AppUser> FilterQuery(this IQueryable<AppUser> query, Query request)
        {
            if (string.IsNullOrEmpty(request.Filter))
                return query;

            var filter = request.Filter.ToUpper();
            return query.Where(g =>
                g.UserName.ToUpper().Contains(filter) || g.Email.ToUpper().Contains(filter)
                );
        }
    }
}
