using FluentAssertions;
using MyRecipeBook.Application.UseCases.Connection.GenerateQRCode;
using Unity.Test.Utils.Entitie;
using Unity.Test.Utils.HashIds;
using Unity.Test.Utils.Repository;
using Unity.Test.Utils.Repository.Code;
using Unity.Test.Utils.UserLogged;
using Xunit;

namespace UseCase.Test.Connection;

public class GenerateQRCode
{
    //[Fact]
    //public async Task Validate_Success()
    //{
    //    (var user, var _) = UserBuilder.Build();

    //    var useCase = CreateUseCase(user);

    //    var result = await useCase.Execute();

    //    result.Should().NotBeNull();
    //    result.qrCode.Should().NotBeNullOrWhiteSpace();

    //    var hashIds = HashIdsBuilder.Instance().Build();
    //    result.userId.Should().Be(hashIds.EncodeLong(user.Id));
    //}

    //private static GenerateQRCodeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    //{
    //    var codeWriteRepository = CodeWriteOnlyRepositoryBuilder.Instance().Build();
    //    var userLogged = UserLoggedBuilder.Instance().RecoverUser(user).Build();
    //    var unityOfWork = UnityOfWorkBuilder.Instance().Build();
    //    var hashIds =  HashIdsBuilder.Instance().Build();

    //    return new GenerateQRCodeUseCase(codeWriteRepository, userLogged, unityOfWork, hashIds);
    //}
}
