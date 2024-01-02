using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Infrastructure.RepositoryAcess;

namespace Web.Api.Test.V1.UnknownErro;

public class MyRecipeBookWebFactoryWithouLoginDependecyInjection<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(MyRecipeBookContext));
                if (descriptor != null)
                    services.Remove(descriptor);

                var loginUseCase = services.SingleOrDefault(d => d.ServiceType == typeof(ILoginUserUseCase));
                if (descriptor != null)
                    services.Remove(loginUseCase);

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
            });
    }
}