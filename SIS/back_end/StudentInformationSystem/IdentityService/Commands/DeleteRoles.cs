using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.Commands
{
    public static class DeleteRoles
    {
        public class Command : IRequest
        {
            public ICollection<string> Ids { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly RoleManager<IdentityRole> roleManager;

            public Handler(RoleManager<IdentityRole> roleManager)
            {
                this.roleManager = roleManager;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                foreach (var Id in request.Ids)
                {
                    var user = await roleManager.FindByIdAsync(Id).ConfigureAwait(false);
                    await roleManager.DeleteAsync(user).ConfigureAwait(false);
                }

                return Unit.Value;
            }
        }

    }
}
