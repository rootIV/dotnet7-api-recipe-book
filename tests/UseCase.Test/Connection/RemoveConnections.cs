using FluentAssertions;
using MyRecipeBook.Application.UseCases.Connection.Remove;
using MyRecipeBook.Exceptions.BaseException;
using MyRecipeBook.Exceptions;
using Unity.Test.Utils.Entitie;
using Unity.Test.Utils.Repository;
using Unity.Test.Utils.Repository.Connection;
using Unity.Test.Utils.UserLogged;
using UseCase.Test.Connection.InlineData;
using Xunit;

namespace UseCase.Test.Connection;

public class RemoveConnections
{
    [Theory]
    [ClassData(typeof(UserConnectionEntitieData))]
    public async Task Validate_Success(long connectedUserIdToRemove, 
        IList<MyRecipeBook.Domain.Entities.User> connections)
    {
        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase(user, connections);

        Func<Task> action = async () => { await useCase.Execute(connectedUserIdToRemove); };

        await action.Should().NotThrowAsync();
    }
    [Fact]
    public async Task Validate_Invalid_User_Erro()
    {
        (var user, var _) = UserBuilder.Build();

        var connections = ConnectionBuilder.Build();

        var useCase = CreateUseCase(user, connections);

        Func<Task> action = async () => { await useCase.Execute(0); };

        await action.Should().ThrowAsync<ValidationErroException>()
            .Where(exception => exception.ErroMessages.Count == 1 && exception.ErroMessages.Contains(ErroMessagesResource.User_Not_Found));
    }

    private static RemoveConnectionsUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user,
        IList<MyRecipeBook.Domain.Entities.User> connections)
    {
        var userLogged = UserLoggedBuilder.Instance().RecoverUser(user).Build();
        var connectionReadRepository = ConnectionReadOnlyRepositoryBuilder.Instance().RecoverConnections(user, connections).Build();
        var connectionWriteRepository = ConnectionWriteOnlyRepositoryBuilder.Instance().Build();
        var unityOfWork = UnityOfWorkBuilder.Instance().Build();

        return new RemoveConnectionsUseCase(userLogged, connectionReadRepository, connectionWriteRepository, unityOfWork);
    }
}
