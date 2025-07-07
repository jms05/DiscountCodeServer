using JMS.Domain.Models.DiscountCodes;
using JMS.Plugins.EntityFramework.Application.EntityTypeConfigurations;
using JMS.Plugins.EntityFramework.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace JMS.Plugins.EntityFramework.Application;
public sealed class ApplicationDbContext : DbContext
{
    public const string SchemaName = "jms";
    public const string MigrationsHistoryTableName = "__MyMigrationsHistory";

    private static readonly SaveChangesInterceptor[] _interceptors
        = new SaveChangesInterceptor[1] { new AuditDatesInterceptor() };

    [ExcludeFromCodeCoverage]
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<DiscountCode> DiscountCodes => Set<DiscountCode>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);

        modelBuilder.SetColumnNamesSnakeCase();

        modelBuilder.ApplyConfiguration(new DiscountCodeConfiguration());

        modelBuilder.SetColumnNamesSnakeCase();

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.AddInterceptors(_interceptors);
}
