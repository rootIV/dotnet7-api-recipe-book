using FluentAssertions;
using MyRecipeBook.Application.UseCases.Dashboard;
using Unity.Test.Utils.Entitie;
using Unity.Test.Utils.Mapper;
using Unity.Test.Utils.Repository.Connection;
using Unity.Test.Utils.Repository.Recipe;
using Unity.Test.Utils.UserLogged;
using Xunit;

namespace UseCase.Test.Dashboard;

public class RecoverDashboard
{
    [Fact]
    public async Task Validate_Without_Recipe_Success()
    {
        (var _user, var _) = UserBuilder.User2();

        var connections = ConnectionBuilder.Build();

        var useCase = CreateUseCase(_user, connections);

        var response = await useCase.Execute(new());

        response.Recipes.Should().HaveCount(0);
    }
    [Fact]
    public async Task Validate_Without_Filter_Success()
    {
        (var _user, var _) = UserBuilder.Build();

        var connections = ConnectionBuilder.Build();

        var recipe = RecipeBuilder.Build(_user);

        var useCase = CreateUseCase(_user, connections, recipe);

        var response = await useCase.Execute(new());

        response.Recipes.Should().HaveCountGreaterThan(0);
    }
    [Fact]
    public async Task Validate_Filter_Title_Success()
    {
        (var _user, var _) = UserBuilder.Build();

        var connections = ConnectionBuilder.Build();

        var recipe = RecipeBuilder.Build(_user);

        var useCase = CreateUseCase(_user, connections, recipe);

        var response = await useCase.Execute(new() { TitleOrIngredient = recipe.Title.ToUpper() });

        response.Recipes.Should().HaveCountGreaterThan(0);
    }
    [Fact]
    public async Task Validate_Filter_Ingredients_Success()
    {
        (var _user, var _) = UserBuilder.Build();

        var connections = ConnectionBuilder.Build();

        var recipe = RecipeBuilder.Build(_user);

        var useCase = CreateUseCase(_user, connections, recipe);

        var response = await useCase.Execute(new() { TitleOrIngredient = recipe.Ingredients.First().Product.ToUpper() });

        response.Recipes.Should().HaveCountGreaterThan(0);
    }
    [Fact]
    public async Task Validate_Filter_Category_Success()
    {
        (var _user, var _) = UserBuilder.Build();

        var connections = ConnectionBuilder.Build();

        var recipe = RecipeBuilder.Build(_user);

        var useCase = CreateUseCase(_user, connections, recipe);

        var response = await useCase.Execute(new() { Category = (MyRecipeBook.Communication.Enum.Category)recipe.Category });

        response.Recipes.Should().HaveCountGreaterThan(0);
    }

    private static DashboardUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, 
        IList<MyRecipeBook.Domain.Entities.User> connectedUsers,
        MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var recipeReadRepository = RecipeReadOnlyRepositoryBuilder.Instance().RecoverUserAllRecipes(recipe).Build();
        var userLogged = UserLoggedBuilder.Instance().RecoverUser(user).Build();
        var mapper = MapperBuilder.Instance();
        var connectionReadRepository = ConnectionReadOnlyRepositoryBuilder.Instance().RecoverConnections(user, connectedUsers).Build();

        return new DashboardUseCase(recipeReadRepository, userLogged, mapper, connectionReadRepository);
    }
}
