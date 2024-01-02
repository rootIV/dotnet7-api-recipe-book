using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Registry;
using MyRecipeBook.Exceptions.BaseException;
using MyRecipeBook.Exceptions;
using Unity.Test.Utils.Entitie;
using Unity.Test.Utils.Mapper;
using Unity.Test.Utils.Repository.Recipe;
using Unity.Test.Utils.Repository;
using Unity.Test.Utils.Requests;
using Unity.Test.Utils.UserLogged;
using Xunit;
using MyRecipeBook.Communication.Enum;

namespace UseCase.Test.Recipe.Registry;

public class RegistryRecipe
{
    [Fact]
    public async Task Validate_Success()
    {
        (var _user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(_user);

        var request = RequestRegistryRecipeBuilder.Build();

        var response = await useCase.Execute(request);

        response.Should().NotBeNull();
        response.Title.Should().Be(request.Title);
        response.Category.Should().Be(request.Category);
        response.PreparationMethod.Should().Be(request.PreparationMethod);
        response.Ingredients.Count.Should().BeGreaterThanOrEqualTo(1);
    }
    [Fact]
    public async Task Validate_Category_Invalid_Erro()
    {
        (var _user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(_user);

        var request = RequestRegistryRecipeBuilder.Build();
        request.Category = (Category)9999;

        Func<Task> response = async () => { await useCase.Execute(request); };

        await response.Should().ThrowAsync<ValidationErroException>()
            .Where(e => e.ErroMessages.Count == 1 && e.ErroMessages.Contains(ErroMessagesResource.User_Recipe_Category_Invalid));
    }
    [Fact]
    public async Task Validate_Recipe_Title_Empty_Erro()
    {
        (var _user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(_user);

        var request = RequestRegistryRecipeBuilder.Build();
        request.Title = string.Empty;

        Func<Task> response = async () => { await useCase.Execute(request); };

        await response.Should().ThrowAsync<ValidationErroException>()
            .Where(e => e.ErroMessages.Count == 1 && e.ErroMessages.Contains(ErroMessagesResource.User_Recipe_Title_Empty));
    }
    [Fact]
    public async Task Validate_Recipe_Ingredients_Empty_Erro()
    {
        (var _user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(_user);

        var request = RequestRegistryRecipeBuilder.Build();
        request.Ingredients.Clear();

        Func<Task> response = async () => { await useCase.Execute(request); };

        await response.Should().ThrowAsync<ValidationErroException>()
            .Where(e => e.ErroMessages.Count == 1 && e.ErroMessages.Contains(ErroMessagesResource.User_Recipe_Ingredients_Empty));
    }
    [Fact]
    public async Task Validate_Recipe_Ingredients_Product_Empty_Erro()
    {
        (var _user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(_user);

        var request = RequestRegistryRecipeBuilder.Build();
        request.Ingredients.First().Product = string.Empty;

        Func<Task> response = async () => { await useCase.Execute(request); };

        await response.Should().ThrowAsync<ValidationErroException>()
            .Where(e => e.ErroMessages.Count == 1 && e.ErroMessages.Contains(ErroMessagesResource.User_Recipe_Ingredients_Product_Empty));
    }
    [Fact]
    public async Task Validate_Recipe_Ingredients_Quantity_Empty_Erro()
    {
        (var _user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(_user);

        var request = RequestRegistryRecipeBuilder.Build();
        request.Ingredients.First().Quantity = string.Empty;

        Func<Task> response = async () => { await useCase.Execute(request); };

        await response.Should().ThrowAsync<ValidationErroException>()
            .Where(e => e.ErroMessages.Count == 1 && e.ErroMessages.Contains(ErroMessagesResource.User_Recipe_Ingredients_Quantity_Empty));
    }
    [Fact]
    public async Task Validate_Recipe_Preparation_Method_Empty_Erro()
    {
        (var _user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(_user);

        var request = RequestRegistryRecipeBuilder.Build();
        request.PreparationMethod = string.Empty;

        Func<Task> response = async () => { await useCase.Execute(request); };

        await response.Should().ThrowAsync<ValidationErroException>()
            .Where(e => e.ErroMessages.Count == 1 && e.ErroMessages.Contains(ErroMessagesResource.User_Recipe_Preparation_Method_Empty));
    }

    private static RegistryRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var mapper = MapperBuilder.Instance();
        var unityOfWork = UnityOfWorkBuilder.Instance().Build();
        var userLogged = UserLoggedBuilder.Instance().RecoverUser(user).Build();
        var recipeWriteRepository = RecipeWriteOnlyRepositoryBuilder.Instance().Build();

        return new RegistryRecipeUseCase(mapper, unityOfWork, userLogged, recipeWriteRepository);
    }
}
