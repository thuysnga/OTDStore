using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OTDStore.Data.EF
{
    public class OTDDbContextFactory : IDesignTimeDbContextFactory<OTDDbContext>
    {
        public OTDDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.json")
                .Build();

            var connectionString = configuration.GetConnectionString("OTDStoreDb");

            var optionsBuilder = new DbContextOptionsBuilder<OTDDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new OTDDbContext(optionsBuilder.Options);
        }
    }
}
