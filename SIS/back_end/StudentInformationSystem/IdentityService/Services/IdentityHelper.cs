using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityService.Commands.ResponseModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace IdentityService.Services
{
    public class IdentityHelper
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly PersistenceContext schoolContext;

        public IdentityHelper(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, PersistenceContext schoolContext)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.schoolContext = schoolContext ?? throw new ArgumentNullException(nameof(schoolContext));
        }

        public async Task<RegisterResponse> CreateUser(AppUser user, string password = null)
        {
            return await CreateWithRolesAndClaims(user, password, UserRole.USER);
        }

        public async Task<RegisterResponse> CreateAdmin(AppUser user, string password)
        {
            return await CreateWithRolesAndClaims(user, password, UserRole.ADMIN);
        }

        private async Task<RegisterResponse> CreateWithRolesAndClaims(AppUser user, string password = null, params UserRole[] roles)
        {
            var response = await Create(user, password);
            var newUser = await userManager.FindByEmailAsync(user.Email).ConfigureAwait(false);

            await GiveRolesAndClaims(newUser, roles); // TODO: update response to reflect updates

            return response;
        }

        private async Task<RegisterResponse> Create(AppUser newUser, string password)
        {
            var userExists = await userManager.FindByNameAsync(newUser.UserName);
            if (userExists != null) return RegisterResponse.Error("User already exists!");

            var result = password != null
                ? await userManager.CreateAsync(newUser, password)
                : await userManager.CreateAsync(newUser);

            return RegisterResponse.From(result);
        }

        private async Task GiveRolesAndClaims(AppUser user, params UserRole[] roles)
        {
            foreach (var role in roles)
            {
                // Create new role if not exists
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                    await roleManager.CreateAsync(new IdentityRole(role.ToString()));


                // give role to user
                if (!await userManager.IsInRoleAsync(user, role.ToString()))
                    await userManager.AddToRoleAsync(user, role.ToString());

                // add student_id claim if student
                if (role == UserRole.USER)
                {
                    var student = await schoolContext.Students.FirstOrDefaultAsync(s => s.Email == user.Email);
                    if (student != null)
                    {
                        await userManager.RemoveClaimAsync(user, new Claim(UserClaims.StudentId, student.Id.ToString()));
                        await userManager.AddClaimAsync(user, new Claim(UserClaims.StudentId, student.Id.ToString()));
                    }
                }
            }
        }

        public async Task CreatePredefinedRoles()
        {
            foreach (var role in UserRole.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                    await roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }

            await AddClaimsToRole(UserRole.ADMIN.ToString(),
                UserClaims.All);

            await AddClaimsToRole(UserRole.USER.ToString(), 
                UserClaims.StudentReadOwn, 
                UserClaims.GroupReadOwn, 
                UserClaims.GradeReadOwn);
        }

        private async Task AddClaimsToRole(string roleName, params string[] claimNames)
        {
            var userRole = await roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            var currentClaims = await roleManager.GetClaimsAsync(userRole);
            foreach (var claimName in claimNames)
            {
                if (currentClaims.All(c => c.Value != claimName))
                    await roleManager.AddClaimAsync(userRole, UserClaims.Claim(claimName));
            }
        }
    }
}
