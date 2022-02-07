//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Persistence.Pipeline
//{
//    public class PersistenceDesignTimeFactory : IDesignTimeDbContextFactory<PersistenceContext>
//    {
//        public PersistenceContext CreateDbContext(string[] args)
//        {
//            IConfigurationRoot configuration = new ConfigurationBuilder()
//                //.AddUserSecrets("f1b4a8eb-2305-4250-8091-d2f5d56acd68")
//                .Build();

//            var builder = new DbContextOptionsBuilder<PersistenceContext>();

//        //FIXME: connectionString == null on Add-Migration
//            var connectionString = configuration.GetConnectionString("DatabaseConnection");

//            builder.UseSqlServer(connectionString);
//            return new PersistenceContext(builder.Options);
//        }
//    }
//}
