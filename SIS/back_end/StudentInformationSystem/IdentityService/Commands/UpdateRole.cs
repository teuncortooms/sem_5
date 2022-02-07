using AutoMapper;
using Core.Domain.Entities;
using IdentityService.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.Commands
{
    public static class UpdateRole
    {
        public class Command : IRequest<AppRoleModel>
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public ICollection<AppClaimModel> RoleClaims { get; set; }
        }

        public class Handler : IRequestHandler<Command, AppRoleModel>
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

            public async Task<AppRoleModel> Handle(Command request, CancellationToken cancellationToken)
            {
                var appRole = await roleManager.FindByIdAsync(request.Id).ConfigureAwait(false);
                appRole.Name = request.Name;
                
                
                var roleClaims = await roleManager.GetClaimsAsync(appRole).ConfigureAwait(false);

                await RemoveClaimsFromRole(
                roleClaims.Where(c => !request.RoleClaims.Any(rc => rc.Name.Equals(c.Value)))
                    .ToList(),appRole);

                await AddRoleClaims(
                request.RoleClaims.Where(rc => !roleClaims.Any(c => c.Value.Equals(rc.Name)))
                    .ToList(),appRole);

                await roleManager.UpdateAsync(appRole).ConfigureAwait(false);

                return await mediator.Send(new GetRole.Query { Id = appRole.Id }, cancellationToken).ConfigureAwait(false);
            }

            public async Task AddRoleClaims(List<AppClaimModel> claims, IdentityRole appRole)
            {
                if (claims.Count == 0)
                    return;

                
                foreach(var claim in claims)
                {
                    var sysClaim = mapper.Map<Claim>(claim);
                    var result = await roleManager.AddClaimAsync(appRole, sysClaim).ConfigureAwait(false);
                    if (!result.Succeeded)
                        throw new Exception(result.Errors.First().Description);
                }

            }

            public async Task RemoveClaimsFromRole(List<Claim> claims, IdentityRole appRole)
            {
                if (claims.Count == 0)
                    return;

                foreach(var sysClaim in claims)
                {
                    var result = await roleManager.RemoveClaimAsync(appRole, sysClaim).ConfigureAwait(false);
                    if (!result.Succeeded)
                        throw new Exception(result.Errors.First().Description);

                }
            }
        }
    }
}
