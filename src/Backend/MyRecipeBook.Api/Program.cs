using MyRecipeBook.Infrastructure;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Infrastructure.Migrations;
using MyRecipeBook.Api.Filter;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application;
using MyRecipeBook.Infrastructure.RepositoryAcess;
using MyRecipeBook.Api.Middleware;
using HashidsNet;
using MyRecipeBook.Api.Filter.Swagger;
using Microsoft.OpenApi.Models;
using MyRecipeBook.Api.WebSockets;
using MyRecipeBook.Api.Filter.LoggedUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MyRecipeBook.Api;

public class Program
{
    protected Program() { }
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.OperationFilter<HashidsOperationFilter>();
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "My Recipe Book API", Version = "1.0" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                In = ParameterLocation.Header,
                Description = "JWT Authorization Header. Example: \"Authorization: Bearer {token}\"",
                BearerFormat = "JWT"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication(builder.Configuration);

        builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionsFilter)));

        builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperConfigurator(provider.GetService<IHashids>()));
        }).CreateMapper());
        builder.Services.AddScoped<IAuthorizationHandler, LoggedUserHandler>();

        builder.Services.AddAuthorization(options => 
        {
            options.AddPolicy("LoggedUser", policy => policy.Requirements.Add(new LoggedUserRequirement()));
        });

        builder.Services.AddScoped<AuthenticatedUserAttribute>();

        //TO-DO: Remove Attributes (Security Warining)
        //builder.Services.AddSignalR(options => 
        //{
        //    options.EnableDetailedErrors = true;
        //});
        builder.Services.AddSignalR();

        builder.Services.AddHealthChecks().AddDbContextCheck<MyRecipeBookContext>();

        var app = builder.Build();

        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            AllowCachingResponses = false,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        UpdateDatabase();

        app.UseMiddleware<CultureMiddleware>();

        app.MapHub<AddConnection>("/addConnection");

        app.Run();

        void UpdateDatabase()
        {
            using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetRequiredService<MyRecipeBookContext>();

            bool? databaseInMemory = context?.Database?.ProviderName?.Equals("Microsoft.EntityFrameworkCore.InMemory");

            if (!databaseInMemory.HasValue || !databaseInMemory.Value)
            {
                var connection = builder.Configuration.GetDatabaseConnectionString();
                var databaseName = builder.Configuration.GetDatabaseName();

                Database.CreateDatabase(connection, databaseName);

                app.MigrateDatabase();
            }
        }
    }
}