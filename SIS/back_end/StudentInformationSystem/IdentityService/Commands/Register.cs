using System;
using System.ComponentModel.DataAnnotations;
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
    public class Register
    {
        public class Command : IRequest<RegisterResponse>
        {
            [EmailAddress]
            [Required(ErrorMessage = "Email is required")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            public string Password { get; set; }

            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public class Handler : IRequestHandler<Command, RegisterResponse>
        {
            protected readonly IdentityHelper identityHelper;

            public Handler(IdentityHelper identityHelper)
            {
                this.identityHelper = identityHelper ?? throw new ArgumentNullException(nameof(identityHelper));
            }

            public async Task<RegisterResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                // NB: CancellationToken not used!
                return await identityHelper.CreateUser(new AppUser(request.Email), request.Password);
            }
        }
    }
}
