using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Core.Application.Interfaces;
using IdentityService;
using IdentityService.Services;
using IdentityService.Validation;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Persistence
{
    public static class IdentityServiceExtensions
    {
        public static void AddIdentityService(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DatabaseConnection"))
                    .EnableSensitiveDataLogging();
            },ServiceLifetime.Transient);

            services.AddMediatR(typeof(IdentityServiceExtensions).Assembly);

            IEnumerable<Assembly> assemblies = new List<Assembly>() { typeof(IdentityServiceExtensions).Assembly };
            services.AddFluentValidation(assemblies);

            services.AddIdentityAndAuthentication(config);

            services.AddScoped<IdentityHelper>();
            services.AddScoped<TokenCreator>();
            services.AddScoped<IClaimsValidator, ClaimsValidator>();
        }

        private static void AddIdentityAndAuthentication(this IServiceCollection services, IConfiguration config)
        {
            // For Identity
            services.AddIdentity<AppUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;

                })
                .AddEntityFrameworkStores<IdentityContext>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultTokenProviders();

            // Adding Authentication
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                // Add social providers
                .AddGoogle("google", options =>
                {
                    var googleAuth = config.GetSection("Auth:Google");
                    options.ClientId = googleAuth["ClientId"];
                    options.ClientSecret = googleAuth["ClientSecret"];
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddMicrosoftAccount(microsoftOptions => // TODO: finish
                {
                    var microsoftAuth = config.GetSection("Auth:Microsoft");
                    microsoftOptions.ClientId = microsoftAuth["ClientId"];
                    microsoftOptions.ClientSecret = microsoftAuth["ClientSecret"];
                    // needed because "organisation accounts only" is set in Azure
                    microsoftOptions.AuthorizationEndpoint = microsoftAuth["AuthorizeUrl"];
                    microsoftOptions.TokenEndpoint = microsoftAuth["TokenUrl"]; // not needed?
                })

                // Adding Jwt Bearer
                .AddJwtBearer(options =>
                {
                    var jwtAuth = config.GetSection("Auth:JWT");
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false; // TODO: set true in production
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtAuth["ValidIssuer"],
                        ValidAudience = jwtAuth["ValidAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuth["SecretKey"]))
                    };
                });

            // policies are registered based on the available claim permissions. Usable in the controllers and handlers.
            services.AddAuthorization(options =>
            {
                foreach (var claimStr in UserClaims.Permissions)
                {
                    var claim = UserClaims.Claim(claimStr);

                    options.AddPolicy(claim.Value, policy => policy.RequireAssertion(context =>
                        context.User.HasClaim(c => (c.Value.Equals(claim.Value) || c.Value.Equals(UserClaims.All))
                                                   && c.Type.Equals("SIS-Claims"))));
                }
            });
        }


        public static void UseIdentityService(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
