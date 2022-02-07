using AutoMapper;
using Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.Queries
{
    public static class GetRole
    {
        public class Query : IRequest<AppRoleModel>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, AppRoleModel>
        {
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly IMapper mapper;

            public Handler(RoleManager<IdentityRole> roleManager, IMapper mapper)
            {
                this.roleManager = roleManager;
                this.mapper = mapper;
            }

            public async Task<AppRoleModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var role = await roleManager.FindByIdAsync(request.Id).ConfigureAwait(false);

                var transform = mapper.Map<AppRoleModel>(role);
                var claims = new List<AppClaimModel>();

                foreach (var claim in await roleManager.GetClaimsAsync(role).ConfigureAwait(false))
                {
                    claims.Add(mapper.Map<AppClaimModel>(claim));
                }
                transform.RoleClaims = claims;

                return transform;

            }
        }
    }
}
