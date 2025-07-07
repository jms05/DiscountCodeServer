using EntityFrameworkRepository.Application.Repositories;
using JMS.Api.Configurations.ErrorHandler;
using JMS.Application.Helpers;
using JMS.Application.Helpers.Implementations;
using JMS.Application.UseCases.DiscountCodes.List;
using JMS.Application.UseCases.RandomGenerator;
using JMS.Domain.Models.DiscountCodes.Repository;
using JMS.Plugins.EntityFramework;
using JMS.Plugins.EntityFramework.Application;
using MediatR;
using Microsoft.Net.Http.Headers;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);

var clientDbOptions = builder.Configuration
    .GetSection(Positions.DatabaseOptions)
    .Get<DatabaseOptions>() ?? new()
    {
        Host = "localhost",
        Port = "5432",
        DatabaseName = "jms-local",
        User = "postgres",
        Password = "postgres",
        LogSensitiveData = false
    };

clientDbOptions.Migrate();

// Context
builder.Services.AddDbContext<ApplicationDbContext>(clientDbOptions.ApplicationDbOptionsAction);

builder.Services.AddSingleton<ICustomLogger,AsyncConsoleLogger>()
            .AddSingleton<IDiscountCodeGenerator, DiscountCodeGenerator>();

// Repos
builder.Services.AddScoped<IAddDiscountCode, DiscountCodeRepository>()
            .AddScoped<IDeleteDiscountCode, DiscountCodeRepository>()
            .AddScoped<IGetDiscountCode, DiscountCodeRepository>()
            .AddScoped<IListDiscountCode, DiscountCodeRepository>()
            .AddScoped<IUpdateDiscountCode, DiscountCodeRepository>();


// Options
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(Positions.DatabaseOptions));

// Bundles
builder.Services.AddMediatR(typeof(ListDiscountCodeQuery).GetTypeInfo().Assembly);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddHeaderPropagation(options =>
{
    options.Headers.Add(HeaderNames.Authorization);
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseHeaderPropagation();

app.Run();
