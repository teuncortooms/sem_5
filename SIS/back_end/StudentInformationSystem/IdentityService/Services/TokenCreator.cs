using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityService.Commands;
using IdentityService.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Services
{
    public class TokenCreator
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public TokenCreator(IConfiguration configuration, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<JwtSecurityToken> CreateToken(AppUser user)
        {
            var issuer = configuration["Auth:JWT:ValidIssuer"];
            var audience = configuration["Auth:JWT:ValidAudience"];
            var expires = GetExpiration();
            var authClaims = await GetAuthClaims(user);
            var signingCredentials = GetSigningCredentials();

            return new JwtSecurityToken(issuer, audience, expires: expires, claims: authClaims,
                signingCredentials: signingCredentials);
        }

        private DateTime GetExpiration()
        {
            if (!double.TryParse(configuration["Auth:JWT:HoursValid"], out double hoursValid))
                throw new InvalidConfigurationException();

            return DateTime.Now.AddHours(hoursValid);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Auth:JWT:SecretKey"]));
            return new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<IEnumerable<Claim>> GetAuthClaims(AppUser user)
        {
            var straightClaims = (await userManager.GetClaimsAsync(user).ConfigureAwait(false)).ToHashSet();

            var idClaims = new HashSet<Claim>
            {
                new Claim(ClaimTypes.Upn, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            // get role claims and claims from roles
            var roleClaims = new HashSet<Claim>();
            var claimsFromRoles = new HashSet<Claim>();
            foreach (var roleName in await userManager.GetRolesAsync(user).ConfigureAwait(false))
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, roleName));

                var role = await roleManager.FindByNameAsync(roleName).ConfigureAwait(false);
                foreach (var claim in await roleManager.GetClaimsAsync(role).ConfigureAwait(false))
                {
                    claimsFromRoles.Add(claim);
                }
            }

            // union all claims
            var result = idClaims.Union(straightClaims).Union(roleClaims).Union(claimsFromRoles);

            // return simpler collection if p_all is present
            var godPermission = UserClaims.Claim(UserClaims.All);
            bool hasGodPermission = result.Any(c => c.Value.Equals(godPermission.Value));

            return hasGodPermission ? idClaims.Union(roleClaims).Append(godPermission) : result;
        }
    }
}