using Application.Infrastructure.IRepository;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Interceptor;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        // Setup Db connection
        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("MyDb"),
                builder => builder.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName));
            options.UseProjectables();
        });
        
        // add scoped
        services.AddScoped<AuditEntitySaveChangeInterceptor>();
        services.AddScoped<IUnitOfWork, UnitOfwork>();
        
        return services;
    }
}