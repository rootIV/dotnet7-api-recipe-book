using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.RecoverById;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;
using Unity.Test.Utils.Entitie;
using Unity.Test.Utils.Mapper;
using Unity.Test.Utils.Repository.Connection;
using Unity.Test.Utils.Repository.Recipe;
using Unity.Test.Utils.UserLogged;
using Xunit;

namespace UseCase.Test.Recipe.RecoverById;

public class RecoverRecipeById
{
    [Fact]
    public async Task Validate_Success()
    {
        (var _user, _) = UserBuilder.Build();

        var connections = ConnectionBuilder.Build();

        var recipe = RecipeBuilder.Build(_user);

        var useCase = CreateUseCase(_user, recipe, connections);

        var response = await useCase.Execute(recipe.Id);

        response.Title.Should().Be(recipe.Title);
        response.Category.Should().Be((MyRecipeBook.Communication.Enum.Category)recipe.Category);
        response.PreparationMethod.Should().Be(recipe.PreparationMethod);
        response.PreparationTime.Should().Be(recipe.PreparationTime);
        response.Ingredients.Should().HaveCount(recipe.Ingredients.Count);
    }
    [Fact]
    public async Task Validate_Recipe_Doesnt_Exists_Erro()
    {
        (var _user, _) = UserBuilder.Build();

        var connections = ConnectionBuilder.Build();

        var recipe = RecipeBuilder.Build(_user);

        var useCase = CreateUseCase(_user, recipe, connections);

        Func<Task> action = async () => { await useCase.Execute(0); };

        await action.Should().ThrowAsync<ValidationErroException>()
            .Where(exception => exception.ErroMessages.Count == 1 &&
            exception.ErroMessages.Contains(ErroMessagesResource.User_Recipe_Not_Found));
    }
    [Fact]
    public async Task Validate_Recipe_Doesnt_Belong_To_User_Logged_Erro()
    {
        (var _user, _) = UserBuilder.Build();
        (var _user2, _) = UserBuilder.User2();

        var connections = ConnectionBuilder.Build();

        var recipe = RecipeBuilder.Build(_user2);

        var useCase = CreateUseCase(_user, recipe, connections);

        Func<Task> action = async () => { await useCase.Execute(recipe.Id); };

        await action.Should().ThrowAsync<ValidationErroException>()
            .Where(exception => exception.ErroMessages.Count == 1 &&
            exception.ErroMessages.Contains(ErroMessagesResource.User_Recipe_Not_Found));
    }

    private static RecoverRecipeByIdUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user,
        MyRecipeBook.Domain.Entities.Recipe recipe,
        IList<MyRecipeBook.Domain.Entities.User> connectedUsers)
    {
        var recipeReadRepository = RecipeReadOnlyRepositoryBuilder.Instance().RecoverRecipeById(recipe).Build();
        var userLogged = UserLoggedBuilder.Instance().RecoverUser(user).Build();
        var mapper = MapperBuilder.Instance();
        var connectionReadRepository = ConnectionReadOnlyRepositoryBuilder.Instance().RecoverConnections(user, connectedUsers).Build();

        return new RecoverRecipeByIdUseCase(recipeReadRepository, userLogged, mapper, connectionReadRepository);
    }
}
