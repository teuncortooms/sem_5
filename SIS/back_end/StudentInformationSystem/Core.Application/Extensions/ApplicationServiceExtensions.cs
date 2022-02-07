using AutoMapper;
using Core.Application.Interfaces;
using Core.Application.Mocks;
using Core.Application.Profiles;
using Core.Application.QueryUtil;
using Core.Application.Validation;
using Core.Domain.Entities;
using FluentValidation;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.Application.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static void AddMockPersistence(this IServiceCollection services)
        {
            services.AddSingleton<IMockDataContext, MockDataContext>();
            services.AddTransient<IRepository<Group>, MockRepository<Group>>();
            services.AddTransient<IRepository<Student>, MockRepository<Student>>();
            services.AddTransient<IRepository<Grade>, MockRepository<Grade>>();
        }

        public static void AddMockGradeRepoOnly(this IServiceCollection services)
        {
            services.AddSingleton<IMockDataContext, MockDataContext>();
            services.AddTransient<IRepository<Grade>, MockRepository<Grade>>();
        }

        public static void AddApplication(this IServiceCollection services, Profile IdentityProfile)
        {
            services.AddMediatR(typeof(ApplicationServiceExtensions).Assembly);
                
            IEnumerable<Assembly> assemblies = new List<Assembly>() {typeof(ApplicationServiceExtensions).Assembly};
            services.AddFluentValidation(assemblies);
            services.AddAutoMapper((sp, cfg) =>
            {
                var scope = sp.CreateScope();
                var groupRepo = scope.ServiceProvider.GetRequiredService<IRepository<Group>>();
                var studentRepo = scope.ServiceProvider.GetRequiredService<IRepository<Student>>();
                var gradesRepo = scope.ServiceProvider.GetRequiredService<IRepository<Grade>>();

                cfg.AddProfile(new GroupMappingProfile(groupRepo));
                cfg.AddProfile(new StudentMappingProfile(studentRepo));
                cfg.AddProfile(new GradeMappingProfile(gradesRepo));
                cfg.AddProfile(IdentityProfile);
                
            }
            , assemblies, ServiceLifetime.Transient);

        }

    }
}
