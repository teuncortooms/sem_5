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
    public static class AddRole
    {
        public class Command : IRequest<AppRoleModel>
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public ICollection<AppClaimModel> RoleClaims { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.RoleClaims).Must(x => x.HaveValidClaims()).WithMessage("Specified claims are not valid.");
            }
        }

        public class Handler : IRequestHandler<Command, AppRoleModel>
        {
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly IMapper mapper;

            public Handler(RoleManager<IdentityRole> roleManager, IMapper mapper)
            {
                this.roleManager = roleManager;
                this.mapper = mapper;
            }
            public async Task<AppRoleModel> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(request.Name)).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    throw new Exception("Role creation failed"); // todo specific error handling.
                }

                var appRole = roleManager.Roles.Single(r => r.Name.Equals(request.Name));

                foreach(var claim in request.RoleClaims)
                {
                    await roleManager.AddClaimAsync(appRole, mapper.Map<Claim>(claim));
                }

                return mapper.Map<AppRoleModel>(appRole);
            }
        }
    }
}
