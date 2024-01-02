using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Exceptions.BaseException;
using MyRecipeBook.Exceptions;
using Unity.Test.Utils.Entitie;
using Unity.Test.Utils.Repository;
using Unity.Test.Utils.Repository.Recipe;
using Unity.Test.Utils.UserLogged;
using Xunit;

namespace UseCase.Test.Recipe.Delete;

public class DeleteRecipe
{
    [Fact]
    public async Task Validate_Success()
    {
        (var _user, _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(_user);

        var useCase = CreateUseCase(_user, recipe);

        Func<Task> action = async () => { await useCase.Execute(recipe.Id); };

        await action.Should().NotThrowAsync();
    }
    [Fact]
    public async Task Validate_Recipe_Doenst_Exists_Erro()
    {
        (var _user, _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(_user);

        var useCase = CreateUseCase(_user, recipe);

        Func<Task> action = async () => { await useCase.Execute(0); };

        await action.Should().ThrowAsync<ValidationErroException>()
            .Where(exception => exception.ErroMessages.Count == 1 &&
            exception.ErroMessages.Contains(ErroMessagesResource.User_Recipe_Not_Found));
    }
    [Fact]
    public async Task Validate_Recipe_Doenst_Belongs_To_User_Logged_Erro()
    {
        (var _user, _) = UserBuilder.Build();
        (var _user2, _) = UserBuilder.User2();

        var recipe = RecipeBuilder.Build(_user2);

        var useCase = CreateUseCase(_user, recipe);

        Func<Task> action = async () => { await useCase.Execute(recipe.Id); };

        await action.Should().ThrowAsync<ValidationErroException>()
            .Where(exception => exception.ErroMessages.Count == 1 &&
            exception.ErroMessages.Contains(ErroMessagesResource.User_Recipe_Not_Found));
    }

    private static DeleteRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user,
        MyRecipeBook.Domain.Entities.Recipe recipe)
    {
        var recipeReadRepository = RecipeReadOnlyRepositoryBuilder.Instance().RecoverRecipeById(recipe).Build();
        var recipeWriteRepository = RecipeWriteOnlyRepositoryBuilder.Instance().Build();
        var userLogged = UserLoggedBuilder.Instance().RecoverUser(user).Build();
        var unityOfWork = UnityOfWorkBuilder.Instance().Build();

        return new DeleteRecipeUseCase(recipeReadRepository, recipeWriteRepository, userLogged, unityOfWork);
    }
}
