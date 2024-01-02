using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Infrastructure.RepositoryAcess;

namespace Web.Api.Test;

public class MyRecipeBookWebFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private User _userWithRecipe;
    private string _passUserWithRecipe;
    private User _userWithoutRecipe;
    private string _passUserWithoutRecipe;
    private User _userWithConnection;
    private string _passUserWithConnection;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(MyRecipeBookContext));
                if (descriptor != null)
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<MyRecipeBookContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopedService = scope.ServiceProvider;
                var database = scopedService.GetRequiredService<MyRecipeBookContext>();

                database.Database.EnsureDeleted();

                (_userWithRecipe, _passUserWithRecipe) = ContextSeedInMemory.Seed(database);
                (_userWithoutRecipe, _passUserWithoutRecipe) = ContextSeedInMemory.SeedWithouRecipe(database);
                (_userWithConnection, _passUserWithConnection) = ContextSeedInMemory.SeedUserWithConnection(database);
            });
    }
    public User RecoverUser()
    {
        return _userWithRecipe;
    }
    public string RecoverPass()
    {
        return _passUserWithRecipe;
    }
    public User RecoverUserWithConnection()
    {
        return _userWithConnection;
    }
    public string RecoverPassUserWithConnection()
    {
        return _passUserWithConnection;
    }
    public User RecoverUserWithoutRecipe()
    {
        return _userWithoutRecipe;
    }
    public string RecoverPassUserWithoutRecipe()
    {
        return _passUserWithoutRecipe;
    }
}