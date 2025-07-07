using JMS.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JMS.Plugins.EntityFramework.Interceptors;
public sealed class AuditDatesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var now = DateTime.UtcNow;

        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            if (entry.Entity is not EntityBase)
            {
                continue;
            }

            if (entry.State == EntityState.Added)
            {
                entry.Property(nameof(EntityBase.CreatedDate)).CurrentValue = now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(EntityBase.UpdatedDate)).CurrentValue = now;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
