using FluentAssertions;
using MyRecipeBook.Application.UseCases.Connection.RefuseConnection;
using Unity.Test.Utils.Entitie;
using Unity.Test.Utils.HashIds;
using Unity.Test.Utils.Repository;
using Unity.Test.Utils.Repository.Code;
using Unity.Test.Utils.UserLogged;
using Xunit;

namespace UseCase.Test.Connection;

public class RefuseConnection
{
    [Fact]
    public async Task Validate_Success()
    {
        var hashids = HashIdsBuilder.Instance().Build();

        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute();

        result.Should().NotBeNullOrWhiteSpace();
        result.Should().Be(hashids.EncodeLong(user.Id));
    }

    private static RefuseConnectionUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var codeWriteRepository = CodeWriteOnlyRepositoryBuilder.Instance().Build();
        var userLogged = UserLoggedBuilder.Instance().RecoverUser(user).Build();
        var unityOfWork = UnityOfWorkBuilder.Instance().Build();
        var hashIds = HashIdsBuilder.Instance().Build();

        return new RefuseConnectionUseCase(codeWriteRepository, userLogged, unityOfWork, hashIds);
    }
}

