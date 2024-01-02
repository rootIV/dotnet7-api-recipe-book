using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Registry;
using MyRecipeBook.Exceptions.BaseException;
using MyRecipeBook.Exceptions;
using Unity.Test.Utils.Cryptography;
using Unity.Test.Utils.Mapper;
using Unity.Test.Utils.Repository.User;
using Unity.Test.Utils.Repository;
using Unity.Test.Utils.Requests;
using Unity.Test.Utils.Token;
using Xunit;

namespace UseCase.Test.User.Registry;

public class RegistryUser
{
    [Fact]
    public async Task Validate_Success()
    {
        var request = RequestRegistryUserBuilder.Build();

        var useCase = CreateUseCase();

        var response = await useCase.Execute(request);
        response.Should().NotBeNull();
        response.Token.Should().NotBeNullOrWhiteSpace();
    }
    [Fact]
    public async Task Validate_Erro_Email_Already_Taken()
    {
        var request = RequestRegistryUserBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        Func<Task> response = async () => { await useCase.Execute(request); };

        await response.Should().ThrowAsync<ValidationErroException>()
            .Where(erro => erro.ErroMessages.Count == 1 && erro.ErroMessages.Contains(ErroMessagesResource.User_Email_Already_Taken));
    }
    [Fact]
    public async Task Validate_Erro_Email_Empty()
    {
        var request = RequestRegistryUserBuilder.Build();
        request.Email = string.Empty;

        var useCase = CreateUseCase();

        Func<Task> response = async () => { await useCase.Execute(request); };

        await response.Should().ThrowAsync<ValidationErroException>()
            .Where(erro => erro.ErroMessages.Count == 1 && erro.ErroMessages.Contains(ErroMessagesResource.User_Email_Empty));
    }

    private static RegistryUserUseCase CreateUseCase(string email = "")
    {
        var writeRepository = UserWriteOnlyRepositoryBuilder.Instance().Build();
        var mapper = MapperBuilder.Instance();
        var unityOfWork = UnityOfWorkBuilder.Instance().Build();
        var encPassword = EncPasswordBuilder.Instance();
        var token = TokenControllerBuilder.Instance();
        var readRepository = UserReadOnlyRepositoryBuilder.Instance().ExistsUserWithEmail(email).Build();

        return new RegistryUserUseCase(readRepository, writeRepository, mapper, unityOfWork, encPassword, token);
    }
}
