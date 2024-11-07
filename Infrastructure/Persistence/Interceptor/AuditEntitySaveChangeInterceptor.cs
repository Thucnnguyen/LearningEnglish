using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Models;

namespace Infrastructure.Persistence.Interceptor;

public class AuditEntitySaveChangeInterceptor(ILogger<AuditEntitySaveChangeInterceptor> logger) : SaveChangesInterceptor
{
    private readonly ILogger<AuditEntitySaveChangeInterceptor> _logger = logger;

    private async Task UpdateEntities(DbContext? dbContext)
    {
        if (dbContext == null) return;

        foreach (var entry in dbContext.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added || entry.HasChangedOwnedEntities())
            {
                entry.Entity.CreatedAt = DateTime.Now;
            }
        }
    }
}
public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State == EntityState.Added);
}
