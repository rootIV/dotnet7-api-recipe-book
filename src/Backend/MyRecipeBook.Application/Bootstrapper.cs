using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Application.Services.Token;
using MyRecipeBook.Application.Services.UserLogged;
using MyRecipeBook.Application.UseCases.Connection.AcceptConnection;
using MyRecipeBook.Application.UseCases.Connection.GenerateQRCode;
using MyRecipeBook.Application.UseCases.Connection.ReadedQRCode;
using MyRecipeBook.Application.UseCases.Connection.Recover;
using MyRecipeBook.Application.UseCases.Connection.RefuseConnection;
using MyRecipeBook.Application.UseCases.Connection.Remove;
using MyRecipeBook.Application.UseCases.Dashboard;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Application.UseCases.Recipe.RecoverById;
using MyRecipeBook.Application.UseCases.Recipe.Registry;
using MyRecipeBook.Application.UseCases.Recipe.Update;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Application.UseCases.User.RecoverProfile;
using MyRecipeBook.Application.UseCases.User.Registry;

namespace MyRecipeBook.Application;

public static class Bootstrapper
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAdditionalPasswordKey(services, configuration);
        AddHashIds(services, configuration);
        AddJwtToken(services, configuration);
        AddUseCases(services);
        AddLoggedUser(services);
    }

    private static void AddAdditionalPasswordKey(this IServiceCollection services, IConfiguration configuration)
    {
        var encPass = configuration.GetRequiredSection("Configurations:Password:EncKey").Value;

        services.AddScoped(options => new EncPassword(encPass));
    }
    private static void AddJwtToken(this IServiceCollection services, IConfiguration configuration)
    {
        var sectionLifeTime = configuration.GetRequiredSection("Configurations:Jwt:TokenLifeTimeMinutes").Value;
        var tokenKey = configuration.GetRequiredSection("Configurations:Jwt:TokenKey").Value;

        services.AddScoped(options => new TokenController(double.Parse(sectionLifeTime), tokenKey));
    }
    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IRegistryUserUseCase, RegistryUserUseCase>()
            .AddScoped<ILoginUserUseCase, LoginUserUseCase>()
            .AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>()
            .AddScoped<IRegistryRecipeUseCase, RegistryRecipeUseCase>()
            .AddScoped<IRecoverRecipeByIdUseCase, RecoverRecipeByIdUseCase>()
            .AddScoped<IUpdateRecipeUseCase, UpdateRecipeUseCase>()
            .AddScoped<IDeleteRecipeUseCase, DeleteRecipeUseCase>()
            .AddScoped<IDashboardUseCase, DashboardUseCase>()
            .AddScoped<IRecoverProfileUseCase, RecoverProfileUseCase>()
            .AddScoped<IGenerateQRCodeUseCase, GenerateQRCodeUseCase>()
            .AddScoped<IReadedQRCodeUseCase, ReadedQRCodeUseCase>()
            .AddScoped<IRefuseConnectionUseCase, RefuseConnectionUseCase>()
            .AddScoped<IAcceptConnectionUseCase, AcceptConnectionUseCase>()
            .AddScoped<IRecoverAllConnectionsUseCase, RecoverAllConnectionsUseCase>()
            .AddScoped<IRemoveConnectionsUseCase, RemoveConnectionsUseCase>();
    }
    private static void AddLoggedUser(this IServiceCollection services)
    {
        services.AddScoped<IUserLogged, UserLogged>();
    }
    private static void AddHashIds(this IServiceCollection services, IConfiguration configuration)
    {
        var salt = configuration.GetRequiredSection("Configurations:HashIds:Salt");

        services.AddHashids(setup =>
        {
            setup.Salt = salt.Value;
            setup.MinHashLength = 3;
        });
    }
}
