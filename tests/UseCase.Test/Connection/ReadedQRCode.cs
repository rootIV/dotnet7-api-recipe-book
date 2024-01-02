using FluentAssertions;
using MyRecipeBook.Application.UseCases.Connection.ReadedQRCode;
using MyRecipeBook.Exceptions.BaseException;
using Unity.Test.Utils.Entitie;
using Unity.Test.Utils.HashIds;
using Unity.Test.Utils.Repository.Code;
using Unity.Test.Utils.Repository.Connection;
using Unity.Test.Utils.UserLogged;
using Xunit;

namespace UseCase.Test.Connection;

public class ReadedQRCode
{
    [Fact]
    public async Task Validate_Success()
    {
        (var qrCodeGenerator, var _) = UserBuilder.User1();
        (var qrCodeReader, var _) = UserBuilder.User2();

        var code = CodeBuilder.Build(qrCodeGenerator);

        var useCase = CreateUseCase(qrCodeReader, code);

        var result = await useCase.Execute(code.Code);

        result.Should().NotBeNull();
        result.ConnectingUser.Should().NotBeNull();

        var hashIds = HashIdsBuilder.Instance().Build();
        result.QRCodeGeneratorUserId.Should().Be(hashIds.EncodeLong(qrCodeGenerator.Id));
    }
    [Fact]
    public async Task Validate_Not_Found_Code_Erro()
    {
        (var qrCodeGenerator, var _) = UserBuilder.User1();
        (var qrCodeReader, var _) = UserBuilder.User2();

        var code = CodeBuilder.Build(qrCodeGenerator);

        var useCase = CreateUseCase(qrCodeReader, code);

        Func<Task> action = async () =>
        {
            await useCase.Execute(Guid.NewGuid().ToString());
        };

        //TO-DO:
        //Create erro message to: Validate_Not_Found_Code_Erro
        await action.Should().ThrowAsync<MyRecipeBookException>()
            .Where(exception => exception.Message.Equals(""));
    }
    [Fact]
    public async Task Validate_Reading_Itself_Code_Erro()
    {
        (var generatorReadingItself, var _) = UserBuilder.Build();

        var code = CodeBuilder.Build(generatorReadingItself);

        var useCase = CreateUseCase(generatorReadingItself, code);

        Func<Task> action = async () =>
        {
            await useCase.Execute(code.Code);
        };

        //TO-DO:
        //Create erro message to: Validate_Reading_Itself_Code_Erro
        await action.Should().ThrowAsync<MyRecipeBookException>()
            .Where(exception => exception.Message.Equals(""));
    }
    [Fact]
    public async Task Validate_Connection_Already_Exists_Erro()
    {
        (var qrCodeGenerator, var _) = UserBuilder.User1();
        (var qrCodeReader, var _) = UserBuilder.User2();

        var code = CodeBuilder.Build(qrCodeGenerator);

        var useCase = CreateUseCase(qrCodeReader, code, qrCodeGenerator.Id, qrCodeReader.Id);

        Func<Task> action = async () =>
        {
            await useCase.Execute(code.Code);
        };

        //TO-DO:
        //Create erro message to: Validate_Connection_Already_Exists_Erro
        await action.Should().ThrowAsync<MyRecipeBookException>()
            .Where(exception => exception.Message.Equals(""));
    }

    private static ReadedQRCodeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user,
    MyRecipeBook.Domain.Entities.Codes code,
    long? userIdA = null,
    long? userIdB = null)
    {
        var codeReadRepository = CodeReadOnlyRepositoryBuilder.Instance().RecoverEntitieCode(code).Build();
        var userLogged = UserLoggedBuilder.Instance().RecoverUser(user).Build();
        var connectionReadRepository = ConnectionReadOnlyRepositoryBuilder.Instance().ExistsConnection(userIdA, userIdB).Build();
        var hashIds = HashIdsBuilder.Instance().Build();

        return new ReadedQRCodeUseCase(codeReadRepository, userLogged, connectionReadRepository, hashIds);
    }
}
