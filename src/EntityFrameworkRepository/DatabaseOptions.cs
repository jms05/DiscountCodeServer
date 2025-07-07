using JMS.Plugins.EntityFramework.Application;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace JMS.Plugins.EntityFramework;

public sealed class DatabaseOptions : IDatabaseOptions
{
    public string? Host { get; set; }
    public string Port { get; set; } = "5432";
    public string? DatabaseName { get; set; }
    public string? User { get; set; }
    public string? Password { get; set; }
    public bool LogSensitiveData { get; set; } = false;

    /// <summary>
    /// Provides the action to be used with DbContextOptionsBuilder
    /// </summary>
    /// <param name="optionsBuilder"></param>
    public void ApplicationDbOptionsAction(DbContextOptionsBuilder optionsBuilder)
    {
        static void npgsqlOptionsAction(NpgsqlDbContextOptionsBuilder pgOptions)
        {
            pgOptions.MigrationsHistoryTable(
                ApplicationDbContext.MigrationsHistoryTableName,
                ApplicationDbContext.SchemaName);
        }

        optionsBuilder.UseNpgsql(BuildConnectionString(), npgsqlOptionsAction);
    }

    /// <summary>
    /// Build options for the application db context
    /// </summary>
    /// <returns></returns>
    public DbContextOptions<ApplicationDbContext> BuildApplicationDbContextOptions()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>();
        ApplicationDbOptionsAction(options);
        return options.Options;
    }

    public void Migrate()
    {
        using var appContext = new ApplicationDbContext(BuildApplicationDbContextOptions());
        appContext.Database.Migrate();
    }
    public string BuildConnectionString()
    {
        if (Host is null)
        {
            throw new ArgumentNullException(nameof(Host));
        }

        if (Port is null)
        {
            throw new ArgumentNullException(nameof(Port));
        }

        if (DatabaseName is null)
        {
            throw new ArgumentNullException(nameof(DatabaseName));
        }

        if (User is null)
        {
            throw new ArgumentNullException(nameof(User));
        }

        if (Password is null)
        {
            throw new ArgumentNullException(nameof(Password));
        }

        return $"Server={Host};Port={Port};Database={DatabaseName};User Id={User};Password={Password}";
    }
}