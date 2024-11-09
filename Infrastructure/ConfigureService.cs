using Application.Infrastructure.IRepository;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Interceptor;
using Infrastructure.Persistence.Repository;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
        // add Assembly 
        services.AddOptions<AssemblyAISetting>()
            .BindConfiguration(AssemblyAISetting.ConfigSectionName)
            .ValidateOnStart();
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<AssemblyAISetting>>().Value);
        
        // add GoogleDriveService
        services.AddOptions<GoogleDriveSetting>()
            .BindConfiguration(GoogleDriveSetting.ConfigSectionName)
            .ValidateOnStart();
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<GoogleDriveSetting>>().Value);        
        return services;

    }
}