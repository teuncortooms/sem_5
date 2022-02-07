using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityService.Commands.ResponseModels;
using IdentityService.Exceptions;
using IdentityService.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Commands
{
    public class Login
    {
        public class Command : IRequest<LoginResponse>
        {
            [Required(ErrorMessage = "Email is required")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            public string Password { get; set; }
        }

        public class Handler : IRequestHandler<Command, LoginResponse>
        {
            private readonly UserManager<AppUser> userManager;
            private readonly TokenCreator tokenCreator;
            private readonly IdentityContext identityCtx;

            public Handler(UserManager<AppUser> userManager, TokenCreator jwtHelper, IdentityContext identityCtx)
            {
                this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                this.tokenCreator = jwtHelper ?? throw new ArgumentNullException(nameof(jwtHelper));
                this.identityCtx = identityCtx;
            }

            public async Task<LoginResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                await CreateDefaultUserIfNeeded().ConfigureAwait(false);

                var user = await userManager.FindByNameAsync(request.Email);

                bool validUserAndPassword = user != null && await userManager.CheckPasswordAsync(user, request.Password);
                if (!validUserAndPassword) return LoginResponse.Unauthorized();

                await GrantAllPermissionsWhenUngranted(user).ConfigureAwait(false);
                

                var token = await tokenCreator.CreateToken(user);
                

                return LoginResponse.Success(token);
            }

            /// <summary>
            /// Check if any user in the database has the p_all claim, if not grant.
            /// Prevents locking out all permissions in the application so that always 1 user has the p_all permissions
            /// Usually the first user of the application recieves this and can perform all actions in the application.
            /// </summary>
            /// <param name="appUser"></param>
            private async Task GrantAllPermissionsWhenUngranted(AppUser appUser)
            {
                var users = await userManager.GetUsersForClaimAsync(UserClaims.Claim(UserClaims.All)).ConfigureAwait(false);
                if (users.Count == 0)
                {
                    var result = await userManager.AddClaimAsync(appUser, UserClaims.Claim(UserClaims.All)).ConfigureAwait(false);
                    if (!result.Succeeded)
                        throw new Exception(result.Errors.ToString());
                }

            }

            /// <summary>
            /// When no users exists, create a default root user so the application can be configured by the first administrator of the application.<br/>
            /// Login: root (root@root), password: P@ssw0rd<br/>
            /// The password should be changed after first use.
            /// </summary>
            /// <returns></returns>
            private async Task CreateDefaultUserIfNeeded()
            {
                if(!identityCtx.Users.Any())
                {
                    await userManager.CreateAsync(new AppUser("root@root", "root") { EmailConfirmed = true },"P@ssw0rd").ConfigureAwait(false);
                }
            }


        }




    }
}
