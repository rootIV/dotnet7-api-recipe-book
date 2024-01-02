using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Repositorys;
using MyRecipeBook.Infrastructure.RepositoryAcess;
using MyRecipeBook.Infrastructure.RepositoryAcess.Repository;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Repositorys.User;
using MyRecipeBook.Domain.Repositorys.Recipe;
using MyRecipeBook.Domain.Repositorys.Code;
using MyRecipeBook.Domain.Repositorys.Connection;

namespace MyRecipeBook.Infrastructure;

public static class Bootstrapper
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configurationManager)
    {
        AddFluentMigrator(services, configurationManager);
        AddRepositorys(services);
        AddUnityOfWork(services);
        AddContext(services, configurationManager);
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configurationManager)
    {
        _ = bool.TryParse(configurationManager.GetSection("Configurations:DatabaseInMemory").Value, out bool databaseInMemory);
        if (!databaseInMemory)
        {
            services.AddFluentMigratorCore().ConfigureRunner(c => c.AddMySql5()
                .WithGlobalConnectionString(configurationManager.GetFullConnection())
                .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure"))
                .For.All());
        }
    }
    private static void AddRepositorys(IServiceCollection services)
    {
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>()
            .AddScoped<IUserReadOnlyRepository, UserRepository>()
            .AddScoped<IUserUpdateOnlyRepository, UserRepository>()
            .AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>()
            .AddScoped<IRecipeReadOnlyRepository, RecipeRepository>()
            .AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>()
            .AddScoped<ICodeWriteOnlyRepository, CodeRepository>()
            .AddScoped<ICodeReadOnlyRepository, CodeRepository>()
            .AddScoped<IConnectionReadOnlyRepository, ConnectionRepository>()
            .AddScoped<IConnectionWriteOnlyRepository, ConnectionRepository>();
    }
    private static void AddUnityOfWork(IServiceCollection services)
    {
        services.AddScoped<IUnityOfWork, UnityOfWork>();
    }
    private static void AddContext(IServiceCollection services, IConfiguration configurationManager)
    {
        _ = bool.TryParse(configurationManager.GetSection("Configurations:DatabaseInMemory").Value, out bool databaseInMemory);
        if (!databaseInMemory)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
            var connectionString = configurationManager.GetFullConnection();

            services.AddDbContext<MyRecipeBookContext>(dbContextOptions =>
            {
                dbContextOptions.UseMySql(connectionString, serverVersion);
            });
        }
    }
}
