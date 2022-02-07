using System;
using Core.Application;
using Core.Application.Extensions;
using Core.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistence;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using IdentityService.Profiles;
using Microsoft.AspNetCore.Http;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDomain();
            services.AddApplication(new IdentityMappingProfile());
            services.AddPersistence(Configuration);
            services.AddIdentityService(Configuration);

            services.AddHttpContextAccessor();
            services.AddTransient<ClaimsPrincipal>(s => {
                var accessor = s.GetService<IHttpContextAccessor>();
                var user = accessor?.HttpContext?.User;
                return user ?? throw new Exception("User not resolved");
            });

            services.AddCors(options => options
                .AddPolicy("MyDefault", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
             ));

            services.AddControllers();

            AddSwaggerWithSecurity(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("MyDefault");

            app.UseIdentityService(); // extension
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddSwaggerWithSecurity(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {

                var apiInfo = new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API"
                };

                options.SwaggerDoc("v1", apiInfo);
                options.CustomSchemaIds(type => type.ToString()); // to avoid schema id for types collision

                // bearer scheme
                var securityScheme = new OpenApiSecurityScheme()
                {
                    Name = "Regular_Authorization",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Enter your token in the text input below.\r\n\r\nExample: \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                var securityRequirements = new OpenApiSecurityRequirement
                {
                    { securityScheme, Array.Empty<string>() }
                };

                options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                options.AddSecurityRequirement(securityRequirements);
            });
        }
    }
}
