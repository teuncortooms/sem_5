using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using IdentityService.Commands.ResponseModels;
using IdentityService.Exceptions;
using IdentityService.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityService.Commands
{
    public class ExternalLogin
    {
        public class Command : IRequest<LoginResponse>
        {
            public string Provider { get; set; }
            public string IdToken { get; set; }
        }

        public class Handler : IRequestHandler<Command, LoginResponse>
        {
            private readonly UserManager<AppUser> userManager;
            private readonly IdentityHelper identityHelper;
            private readonly TokenCreator tokenCreator;
            private readonly IConfiguration configuration;

            public Handler(UserManager<AppUser> userManager, IdentityHelper identityHelper, TokenCreator tokenCreator,
                IConfiguration configuration)
            {
                this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                this.identityHelper = identityHelper ?? throw new ArgumentNullException(nameof(identityHelper));
                this.tokenCreator = tokenCreator ?? throw new ArgumentNullException(nameof(tokenCreator));
                this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            }

            public async Task<LoginResponse> Handle(Command command, CancellationToken cancellationToken)
            {
                // see below for example tokens

                return command.Provider switch
                {
                    "MICROSOFT" => await HandleMicrosoft(command),
                    "GOOGLE" => await HandleGoogle(command),
                    _ => throw new NotImplementedException("Provider not recognized.")
                };
            }

            private async Task<LoginResponse> HandleMicrosoft(Command command)
            {
                try
                {
                    var validationParameters = await CreateMsValidationPars();
                    var tokenHandler = new JwtSecurityTokenHandler();
                    tokenHandler.ValidateToken(command.IdToken, validationParameters, out var validatedToken);

                    string subject = ((JwtSecurityToken)validatedToken).Subject; // unsafe cast?
                    string email = ((JwtSecurityToken)validatedToken).Claims.FirstOrDefault(x => x.Type == "email")?.Value;

                    var user = await FindOrCreateUser(command.Provider, subject, email);
                    var token = await tokenCreator.CreateToken(user);

                    return LoginResponse.Success(token);
                }
                catch (Exception e)
                {
                    return LoginResponse.Unauthorized(e.Message);
                }
            }

            private async Task<TokenValidationParameters> CreateMsValidationPars()
            {
                var myMsConfig = configuration.GetSection("Auth:Microsoft");

                var openIdConfigManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    myMsConfig["metaDataUrl"], new OpenIdConnectConfigurationRetriever());
                var openIdConfig = await openIdConfigManager.GetConfigurationAsync();

                return new TokenValidationParameters
                {
                    ValidIssuer = myMsConfig["Issuer"],
                    ValidAudience = myMsConfig["ClientId"],
                    IssuerSigningKeys = openIdConfig.SigningKeys,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(myMsConfig["ClientSecret"])),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateTokenReplay = true
                };
            }

            private async Task<LoginResponse> HandleGoogle(Command command)
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { configuration["Auth:Google:ClientId"] }
                };
                var tokenPayload = await GoogleJsonWebSignature.ValidateAsync(command.IdToken, settings);

                var user = await FindOrCreateUser(command.Provider, tokenPayload.Subject, tokenPayload.Email);
                var token = await tokenCreator.CreateToken(user);

                return LoginResponse.Success(token);
            }

            private async Task<AppUser> FindOrCreateUser(string provider, string subject, string email)
            {
                // find user
                var user = await userManager.FindByLoginAsync(provider, subject)
                           ?? await userManager.FindByEmailAsync(email);

                // or create new user
                if (user == null)
                {
                    user = new AppUser(email);
                    var result = await identityHelper.CreateUser(user);
                    if (result.StatusCode == RegistrationStatus.Error)
                        throw new CouldNotCreateUserException("Could not create new user.");
                }

                // connect user to external login info
                var info = new UserLoginInfo(provider, subject, provider);
                await userManager.AddLoginAsync(user, info);

                return user;
            }
        }
    }


}


//{
//    "iss": "accounts.google.com",
//  "azp": "311001800198-ppa5ol5kn8phtoivv2oomu4e3316bsal.apps.googleusercontent.com",
//  "aud": "311001800198-ppa5ol5kn8phtoivv2oomu4e3316bsal.apps.googleusercontent.com",
//  "sub": "109193248027604515011",
//  "email": "teuncortooms@gmail.com",
//  "email_verified": true,
//  "at_hash": "7uQOTjDIzqAVCUVS4TskAA",
//  "name": "Teun Cortooms",
//  "picture": "https://lh3.googleusercontent.com/a-/AOh14GgrmTJhOmNuwH3C4yKJcxUAhT2iVyQNzKyDhN8dGuY=s96-c",
//  "given_name": "Teun",
//  "family_name": "Cortooms",
//  "locale": "en-GB",
//  "iat": 1639092192,
//  "exp": 1639095792,
//  "jti": "bc3b8ba03021e5d123864bf0282dd6392c9a50b5"
//}

//{
//    "aud": "2626695a-e45d-4379-895a-6f759b5051df",
//  "iss": "https://login.microsoftonline.com/0172c9f5-f568-42ac-9eb8-24ef84881faa/v2.0",
//  "iat": 1639091932,
//  "nbf": 1639091932,
//  "exp": 1639095832,
//  "aio": "ATQAy/8TAAAA7SORcFn3SK9FvQUZYCjT5n+vUMQd/Qj30kioH0YvtOMfYioup6r9uG4EQt5XW3vj",
//  "email": "t.cortooms@fhict.nl",
//  "name": "Cortooms,Teun T.",
//  "nonce": "c1176e3c-5e82-4a06-9d7a-517d773f52d2",
//  "oid": "60de89d7-e4de-4290-a141-68be80c54ff0",
//  "preferred_username": "I884573@fhict.nl",
//  "rh": "0.AVwA9clyAWj1rEKeuCTvhIgfqlppJiZd5HlDiVpvdZtQUd9cANM.",
//  "sid": "36adc941-b658-46d0-bb8b-244011e11d47",
//  "sub": "NOgzvVvhsxI7v4LyZeAcKZPkCBnpmxurRAHmW9CsYJc",
//  "tid": "0172c9f5-f568-42ac-9eb8-24ef84881faa",
//  "uti": "wYbYGpsAUUiKyBQu5m81AA",
//  "ver": "2.0"
//}

