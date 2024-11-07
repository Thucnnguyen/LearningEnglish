using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Factory;

public class ApplicationFactoryContext : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var parentPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        var basePath = Path.Combine(parentPath, "LearnEnglishApi");
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();
        var builder = new DbContextOptionsBuilder<ApplicationContext>();
        var connectionString = configuration.GetConnectionString("MyDb");

        builder.UseNpgsql(connectionString);

        return new ApplicationContext(builder.Options);
    }
}