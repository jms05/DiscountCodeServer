using EntityFrameworkRepository.Application.Repositories;
using GrpcApi.ErrorHandler;
using JMS.Application.Helpers;
using JMS.Application.Helpers.Implementations;
using JMS.Application.UseCases.DiscountCodes.List;
using JMS.Application.UseCases.RandomGenerator;
using JMS.Domain.Models.DiscountCodes.Repository;
using JMS.GrpcApi.Services;
using JMS.Plugins.EntityFramework;
using JMS.Plugins.EntityFramework.Application;
using MediatR;
using Microsoft.Net.Http.Headers;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);

var clientDbOptions = new DatabaseOptions()
    {
        Host = "localhost",
        Port = "5432",
        DatabaseName = "jms-local",
        User = "postgres",
        Password = "postgres",
        LogSensitiveData = false
    };

clientDbOptions.Migrate();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(clientDbOptions.ApplicationDbOptionsAction);

// Helpers
builder.Services.AddSingleton<ICustomLogger, AsyncConsoleLogger>()
                .AddSingleton<IDiscountCodeGenerator, DiscountCodeGenerator>();

// Repositories
builder.Services.AddScoped<IAddDiscountCode, DiscountCodeRepository>()
                .AddScoped<IDeleteDiscountCode, DiscountCodeRepository>()
                .AddScoped<IGetDiscountCode, DiscountCodeRepository>()
                .AddScoped<IListDiscountCode, DiscountCodeRepository>()
                .AddScoped<IUpdateDiscountCode, DiscountCodeRepository>();

// Options
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(Positions.DatabaseOptions));

// MediatR
builder.Services.AddMediatR(typeof(ListDiscountCodeQuery).GetTypeInfo().Assembly);

// Add gRPC
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ExceptionInterceptorGRPC>();
});


var app = builder.Build();

// Map gRPC endpoints
app.MapGrpcService<DiscountCodeServiceImpl>();

app.Run();
