using AutoMapper;
using Core.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityService.Extensions;

namespace IdentityService.Commands
{
    public static class AddUser
    {
        public class Command : IRequest<AppUserModel>
        {
            public string Id { get; set; }
            public string LoginName { get; set; }
            public string EMail { get; set; }
            public string Password { get; set; }
            public ICollection<AppClaimModel> Claims { get; set; }
            public ICollection<AppRoleModel> Roles { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(RoleManager<IdentityRole> roleManager)
            {
                RuleFor(x => x.Claims).Must(x => x.HaveValidClaims()).WithMessage("Specified claims are not valid.");
                RuleFor(x => x.Roles).MustAsync(async (r, x, cc, token) => await x.HaveValidRoles(roleManager, token).ConfigureAwait(false))
                    .WithMessage("Specified roles are not valid.");
            }
        }


        public class Handler : IRequestHandler<Command, AppUserModel>
        {
            private readonly UserManager<AppUser> userManager;
            private readonly IMapper mapper;
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly IdentityContext identityContext;

            public Handler(UserManager<AppUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager, IdentityContext identityContext)
            {
                this.userManager = userManager;
                this.mapper = mapper;
                this.roleManager = roleManager;
                this.identityContext = identityContext;
            }


            public async Task<AppUserModel> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await userManager.CreateAsync(new AppUser(request.EMail, request.LoginName), request.Password).ConfigureAwait(false);
                if(!result.Succeeded)
                {
                    throw new Exception("User creation failed"); // todo specific error handling.
                }

                var appUser = await userManager.FindByNameAsync(request.LoginName).ConfigureAwait(false);

                foreach (var claim in request.Claims)
                {
                    await userManager.AddClaimAsync(appUser, mapper.Map<Claim>(claim)).ConfigureAwait(false);
                }

                foreach (var role in request.Roles)
                {
                    await userManager.AddToRoleAsync(appUser, role.Name).ConfigureAwait(false);
                }

                return mapper.Map<AppUserModel>(appUser);
            }
        }
    }
}
