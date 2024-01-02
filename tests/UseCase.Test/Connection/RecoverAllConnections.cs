using FluentAssertions;
using MyRecipeBook.Application.UseCases.Connection.Recover;
using Unity.Test.Utils.Entitie;
using Unity.Test.Utils.Mapper;
using Unity.Test.Utils.Repository.Connection;
using Unity.Test.Utils.Repository.Recipe;
using Unity.Test.Utils.UserLogged;
using Xunit;

namespace UseCase.Test.Connection;

public class RecoverAllConnections
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public async Task Validate_Success(int recipeQuantity)
    {
        (var user, var _) = UserBuilder.Build();

        var connections = ConnectionBuilder.Build();

        var useCase = CreateUseCase(user, connections, recipeQuantity);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Users.Should().NotBeEmpty();
        result.Users.Should().HaveCount(connections.Count);

        result.Users.Should().SatisfyRespectively(first =>
        {
            first.Id.Should().NotBeNullOrWhiteSpace();
            first.Name.Should().Be(connections.First().Name);
            first.RecipesQuantity.Should().Be(recipeQuantity);
        });
    }

    private static RecoverAllConnectionsUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user,
        IList<MyRecipeBook.Domain.Entities.User> connections,
        int recipeQuantity)
    {
        var userLogged = UserLoggedBuilder.Instance().RecoverUser(user).Build();
        var connectionReadRepository = ConnectionReadOnlyRepositoryBuilder.Instance().RecoverConnections(user, connections).Build();
        var recipeReadRepository = RecipeReadOnlyRepositoryBuilder.Instance().RecipeQuantity(recipeQuantity).Build();
        var mapper = MapperBuilder.Instance();

        return new RecoverAllConnectionsUseCase(userLogged, connectionReadRepository, recipeReadRepository, mapper);
    }
}
