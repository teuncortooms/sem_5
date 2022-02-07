using Core.Application.Interfaces;
using Core.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Config;
using Persistence.Pipeline;
using Persistence.Repositories;
using System.Diagnostics;

namespace Persistence
{
    public static class PersistenceServiceExtensions
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            SqlAuthenticationProvider.SetProvider(SqlAuthenticationMethod.ActiveDirectoryInteractive, new SqlAppAuthenticationProvider());

            services.AddDbContext<PersistenceContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DatabaseConnection"))
                    .EnableSensitiveDataLogging();
            });

            services.AddTransient<IRepository<Student>, StudentsRepository>();
            services.AddTransient<IRepository<Group>, GroupsRepository>();
            services.AddTransient<IRepository<Teacher>, Repository<Teacher>>();
            services.AddTransient<IRepository<Grade>, Repository<Grade>>();
        }
    }
}
