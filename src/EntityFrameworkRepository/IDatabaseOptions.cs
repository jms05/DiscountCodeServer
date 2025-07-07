using JMS.Plugins.EntityFramework.Application;
using Microsoft.EntityFrameworkCore;

namespace JMS.Plugins.EntityFramework;
public interface IDatabaseOptions
{
    string? DatabaseName { get; set; }
    string? Host { get; set; }
    bool LogSensitiveData { get; set; }
    string? Password { get; set; }
    string Port { get; set; }
    string? User { get; set; }

    void ApplicationDbOptionsAction(DbContextOptionsBuilder optionsBuilder);
    DbContextOptions<ApplicationDbContext> BuildApplicationDbContextOptions();
    string BuildConnectionString();
}