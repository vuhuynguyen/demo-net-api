using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace Infrastructure.DbMigrations
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var connectionString = args != null && args.Length > 0 ? args[0] : "";
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.migration.json", optional: true, reloadOnChange: true);

                var configuration = config.Build();
                connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", DatabaseContext.SCHEMA);
                sqlOptions.MigrationsAssembly("Infrastructure.DbMigrations");
            });

            return new DatabaseContext(builder.Options);
        }
    }
}
