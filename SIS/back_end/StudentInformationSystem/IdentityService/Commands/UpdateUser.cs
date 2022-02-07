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
using System.Transactions;

namespace IdentityService.Commands
{
    public static class UpdateUser
    {
        public class Command : IRequest<AppUserModel>
        {
            public string Id { get; set; }
            public string LoginName { get; set; }
            public string EMail { get; set; }
            public ICollection<AppClaimModel> Claims { get; set; }
            public ICollection<AppRoleModel> Roles { get; set; }
        }

        public class Handler : IRequestHandler<Command, AppUserModel>
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

            public async Task<AppUserModel> Handle(Command request, CancellationToken cancellationToken)
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions() { Timeout = TimeSpan.FromSeconds(600), IsolationLevel = IsolationLevel.RepeatableRead },
                    TransactionScopeAsyncFlowOption.Enabled))
                {

                    var appUser = await userManager.FindByIdAsync(request.Id);
                    appUser.UserName = request.LoginName;
                    appUser.Email = request.EMail;

                    await userManager.UpdateAsync(appUser);

                    var userRoles = await userManager.GetRolesAsync(appUser).ConfigureAwait(false);
                    var userClaims = await userManager.GetClaimsAsync(appUser).ConfigureAwait(false);

                    userClaims.Where(c => !request.Claims.Any(rc => rc.Name.Equals(c.Value)))
                        .ToList().ForEach(async c => await userManager.RemoveClaimAsync(appUser, c));
    
                    request.Claims.Where(rc => !userClaims.Any(c => c.Value.Equals(rc.Name)))
                        .ToList().ForEach(async c => await userManager.AddClaimAsync(appUser, mapper.Map<Claim>(c)));

                    await RemoveUserFromRoles(userRoles.Where(r => !request.Roles.Any(rr => rr.Name.Equals(r)))
                        .ToList(), appUser);

                    await AddUserRoles(request.Roles.Where(rr => !userRoles.Any(r => r.Equals(rr.Name)))
                        .Select(r=>r.Name).ToList(),appUser);

                    scope.Complete();
                    
                }

                return await mediator.Send(new GetUser.Query { Id = request.Id }, cancellationToken).ConfigureAwait(false);

            }

            public async Task<IdentityResult> AddUserRoles(List<string> roles, AppUser appUser)
            {
                if (roles.Count == 0) 
                    return null;
                return await userManager.AddToRolesAsync(appUser, roles).ConfigureAwait(false);
            }

            public async Task<IdentityResult> RemoveUserFromRoles(List<string> roles, AppUser appUser)
            {
                if (roles.Count == 0)
                    return null;
                return await userManager.RemoveFromRolesAsync(appUser, roles).ConfigureAwait(false);
            }
        }
    }
}
