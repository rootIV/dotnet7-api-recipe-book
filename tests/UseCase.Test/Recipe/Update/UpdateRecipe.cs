using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Update;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;
using Unity.Test.Utils.Entitie;
using Unity.Test.Utils.Mapper;
using Unity.Test.Utils.Repository;
using Unity.Test.Utils.Repository.Recipe;
using Unity.Test.Utils.Requests;
using Unity.Test.Utils.UserLogged;
using Xunit;

namespace UseCase.Test.Recipe.Update;

public class UpdateRecipe
{
    [Fact]
    public async Task Validate_Success()
    {
        (var _user, _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(_user);

        var useCase = CreateUseCase(recipe, _user);

        var request = RequestRegistryRecipeBuilder.Build();

        await useCase.Execute(recipe.Id, request);

        Func<Task> action = async () => { await useCase.Execute(recipe.Id, request); };

        await action.Should().NotThrowAsync();
    }
    [Fact]
    public async Task Validate_Empty_Ingredients_Erro()
    {
        (var _user, _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(_user);

        var useCase = CreateUseCase(recipe, _user);

        var request = RequestRegistryRecipeBuilder.Build();
        request.Ingredients.Clear();

        Func<Task> action = async () => { await useCase.Execute(recipe.Id, request); };

        await action.Should().ThrowAsync<ValidationErroException>()
            .Where(exception => exception.ErroMessages.Count == 1 &&
            exception.ErroMessages.Contains(ErroMessagesResource.User_Recipe_Ingredients_Empty));
    }
    [Fact]
    public async Task Validate_Recipe_Doesnt_Exists_Erro()
    {
        (var _user, _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(_user);

        var useCase = CreateUseCase(recipe, _user);

        var request = RequestRegistryRecipeBuilder.Build();

        Func<Task> action = async () => { await useCase.Execute(0, request); };

        await action.Should().ThrowAsync<ValidationErroException>()
            .Where(exception => exception.ErroMessages.Count == 1 &&
            exception.ErroMessages.Contains(ErroMessagesResource.User_Recipe_Not_Found));
    }
    [Fact]
    public async Task Validate_Recipe_Doesnt_Belongs_To_User_Logged_Erro()
    {
        (var _user, _) = UserBuilder.Build();
        (var _user2, _) = UserBuilder.User2();

        var recipe = RecipeBuilder.Build(_user2);

        var useCase = CreateUseCase(recipe, _user);

        var request = RequestRegistryRecipeBuilder.Build();

        Func<Task> action = async () => { await useCase.Execute(recipe.Id, request); };

        await action.Should().ThrowAsync<ValidationErroException>()
            .Where(exception => exception.ErroMessages.Count == 1 &&
            exception.ErroMessages.Contains(ErroMessagesResource.User_Recipe_Not_Found));
    }

    private static UpdateRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.Recipe recipe,
        MyRecipeBook.Domain.Entities.User user)
    {
        var recipeUpdateRepository = RecipeUpdateOnlyRepositoryBuilder.Instance().RecoverById(recipe).Build();
        var userLogged = UserLoggedBuilder.Instance().RecoverUser(user).Build();
        var mapper = MapperBuilder.Instance();
        var unityOfWork = UnityOfWorkBuilder.Instance().Build();

        return new UpdateRecipeUseCase(recipeUpdateRepository, userLogged, mapper, unityOfWork);
    }
}
