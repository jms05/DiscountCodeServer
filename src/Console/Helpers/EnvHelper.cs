using EntityFrameworkRepository.Application.Repositories;
using JMS.Application.Helpers;
using JMS.Application.Helpers.Implementations;
using JMS.Application.UseCases.DiscountCodes.List;
using JMS.Application.UseCases.RandomGenerator;
using JMS.Domain.Models.DiscountCodes.Repository;
using JMS.Plugins.EntityFramework;
using JMS.Plugins.EntityFramework.Application;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ConsoleInteface.Helpers;

internal static class EnvHelper
{
    public static IMediator Build(DatabaseOptions? clientDbOptions = null)
    {
        clientDbOptions = new DatabaseOptions
        {
            DatabaseName = "jms-local",
            Host = "localhost",
            Port = "5432",
            User = "postgres",
            Password = "postgres",
            LogSensitiveData = true
        };

        clientDbOptions.Migrate();

        var serviceCollection = new ServiceCollection()
            .AddDbContext<ApplicationDbContext>(clientDbOptions.ApplicationDbOptionsAction)
            .AddSingleton<ICustomLogger, AsyncConsoleLogger>()
            .AddSingleton<IDiscountCodeGenerator, DiscountCodeGenerator>()

            //Repos Add Here
            .AddScoped<IAddDiscountCode, DiscountCodeRepository>()
            .AddScoped<IDeleteDiscountCode, DiscountCodeRepository>()
            .AddScoped<IGetDiscountCode, DiscountCodeRepository>()
            .AddScoped<IListDiscountCode, DiscountCodeRepository>()
            .AddScoped<IUpdateDiscountCode, DiscountCodeRepository>()

            .AddMediatR(typeof(ListDiscountCodeQuery).GetTypeInfo().Assembly)

            .BuildServiceProvider();

        return serviceCollection.GetRequiredService<IMediator>();
    }
}
