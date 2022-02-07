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
    public static class GetUser
    {
        public class Query : IRequest<AppUserModel>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, AppUserModel>
        {
            private readonly IMapper mapper;
            private readonly UserManager<AppUser> userManager;
            private readonly RoleManager<IdentityRole> roleManager;

            public Handler(IMapper mapper, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                this.mapper = mapper;
                this.userManager = userManager;
                this.roleManager = roleManager;
            }

            public async Task<AppUserModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var usr = userManager.Users.Single(u => u.Id.Equals(request.Id));
                var transform = mapper.Map<AppUserModel>(usr);
                var claims = new List<AppClaimModel>();
                var roles = new List<AppRoleModel>();

                foreach (var claim in await userManager.GetClaimsAsync(usr).ConfigureAwait(false))
                {
                    claims.Add(mapper.Map<AppClaimModel>(claim));
                }
                transform.Claims = claims;

                foreach (var role in await userManager.GetRolesAsync(usr).ConfigureAwait(false))
                {
                    var roleObj = roleManager.Roles.Single(r => r.Name.Equals(role));
                    var roleClaims = new List<AppClaimModel>();
                    // role claims
                    foreach (var claim in await roleManager.GetClaimsAsync(roleObj).ConfigureAwait(false))
                    {
                        roleClaims.Add(mapper.Map<AppClaimModel>(claim));
                    }

                    var transformRole = mapper.Map<AppRoleModel>(roleObj);
                    transformRole.RoleClaims = roleClaims;
                    roles.Add(transformRole);
                }
                transform.Roles = roles;

                return transform;
            }
        }
    }
}
