using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityService.Commands.ResponseModels;
using IdentityService.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Commands
{
    public class RegisterAdmin
    {
        public class Command : Register.Command { }

        public class Handler : IRequestHandler<Command, RegisterResponse>
        {
            private readonly IdentityHelper identityHelper;

            public Handler(IdentityHelper identityHelper)
            {
                this.identityHelper = identityHelper ?? throw new ArgumentNullException(nameof(identityHelper));
            }

            public async Task<RegisterResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                return await identityHelper.CreateAdmin(new AppUser(request.Email), request.Password);
            }
        }
    }
}
