using MyRecipeBook.Application.UseCases.Connection.RefuseConnection;
using Unity.Test.Utils.Entitie;
using Unity.Test.Utils.HashIds;
using Unity.Test.Utils.Repository.Code;
using Unity.Test.Utils.Repository;
using Unity.Test.Utils.UserLogged;
using Xunit;
using MyRecipeBook.Application.UseCases.Connection.AcceptConnection;
using Unity.Test.Utils.Repository.Connection;
using FluentAssertions;

namespace UseCase.Test.Connection;

public class AcceptConnection
{
    [Fact]
    public async Task Validate_Success()
    {
        (var user, var _) = UserBuilder.Build();
        (var userToConnectId, var _) = UserBuilder.User2();

        var useCase = CreateUseCase(user);

        var hashids = HashIdsBuilder.Instance().Build();
        var result = await useCase.Execute(hashids.EncodeLong(userToConnectId.Id));

        result.Should().NotBeNullOrWhiteSpace();
        result.Should().Be(hashids.EncodeLong(user.Id));
    }

    private static AcceptConnectionUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var codeWriteRepository = CodeWriteOnlyRepositoryBuilder.Instance().Build();
        var userLogged = UserLoggedBuilder.Instance().RecoverUser(user).Build();
        var unityOfWork = UnityOfWorkBuilder.Instance().Build();
        var hashIds = HashIdsBuilder.Instance().Build();
        var connectionWriteRepository = ConnectionWriteOnlyRepositoryBuilder.Instance().Build();

        return new AcceptConnectionUseCase(codeWriteRepository, userLogged, unityOfWork, hashIds, connectionWriteRepository);
    }
}
