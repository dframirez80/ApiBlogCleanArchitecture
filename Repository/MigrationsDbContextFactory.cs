using Repository.Repositories.EntityDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Repository
{
    public class MigrationsDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        private const string CONNECTION_NAME = "ApiBlogCADB";
        private const string APPSETTINGS_FILENAME = "dbsettings.json";

        public AppDbContext CreateDbContext(string[] args)
        {
            var connectionString = GetConnectionString(CONNECTION_NAME);

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }

        private string GetConnectionString(string key)
        {
            var appSettingsPath = Path.Combine(Environment.CurrentDirectory, APPSETTINGS_FILENAME);

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(appSettingsPath);
            var rootBuilded = configBuilder.Build();

            return rootBuilded.GetConnectionString(key);
        }
    }
}
