using Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.Extensions
{
    public static class ValidationExtensions
    {
        public static bool HaveValidClaims(this ICollection<AppClaimModel> claims)
        {
            foreach (var claim in claims)
            {
                if (!UserClaims.Permissions.Contains(claim.Name))
                    return false;
            }
            return true;
        }

        public static async Task<bool> HaveValidRoles(this ICollection<AppRoleModel> roles, RoleManager<IdentityRole> roleManager, CancellationToken cancellationToken)
        {
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name).ConfigureAwait(false))
                    return false;
            }
            return true;
        }
    }
}
