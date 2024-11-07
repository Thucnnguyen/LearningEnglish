using Infrastructure.Persistence.Interceptor;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Persistence.Context;

public class ApplicationContext: DbContext
{
    private readonly AuditEntitySaveChangeInterceptor _saveChangeInterceptor ;

     public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
     {
     }
    // public ApplicationContext(DbContextOptions<ApplicationContext> options,
    //                           AuditEntitySaveChangeInterceptor saveChangeInterceptor) : base(options)
    // {
    //     _saveChangeInterceptor = saveChangeInterceptor;
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //add interceptor
        optionsBuilder.AddInterceptors(_saveChangeInterceptor);
    }

    
    public DbSet<Audio> Audios => Set<Audio>();
    public DbSet<Script> Scripts => Set<Script>();    
    
}