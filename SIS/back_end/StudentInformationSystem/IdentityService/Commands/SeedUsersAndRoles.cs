using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Entities;
using IdentityService.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Commands
{
    public static class SeedUsersAndRoles
    {
        public class Command : IRequest
        {
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IdentityHelper helper;

            public Handler(IdentityHelper helper)
            {
                this.helper = helper ?? throw new ArgumentNullException(nameof(helper));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                // seed roles
                await helper.CreatePredefinedRoles();

                // seed users
                var admins = new[]
                {
                    new[]{"root@root", "P@ssw0rd" },
                    new[]{"teuncortooms@gmail.com", "test1234!" }
                };

                var students = new[]
                {
                    new[]{"test@test.nl", "test1234!" },
                };

                foreach (var line in admins)
                {
                    var admin = new AppUser(line[0]) { EmailConfirmed = true };
                    await helper.CreateAdmin(admin, line[1]);
                }

                foreach (var line in students)
                {
                    var student = new AppUser(line[0]) { EmailConfirmed = true };
                    await helper.CreateUser(student, line[1]);
                }

                return Unit.Value;
            }
        }
    }
}
