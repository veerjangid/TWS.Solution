using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TWS.Data.Context
{
    /// <summary>
    /// Design-time factory for TWSDbContext
    /// Used by EF Core tools (migrations) when they cannot resolve DbContext from DI
    /// Reference: https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation
    /// </summary>
    public class TWSDbContextFactory : IDesignTimeDbContextFactory<TWSDbContext>
    {
        public TWSDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TWSDbContext>();

            // Use a temporary connection string for migrations
            // This will not actually connect to the database during migration creation
            var connectionString = "Server=localhost;Database=TWS_Investment;User=root;Password=Mp@9166140829;Port=3306;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.Parse("8.0.0-mysql"));

            return new TWSDbContext(optionsBuilder.Options);
        }
    }
}