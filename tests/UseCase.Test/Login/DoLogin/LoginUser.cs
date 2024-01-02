using FluentAssertions;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;
using Unity.Test.Utils.Cryptography;
using Unity.Test.Utils.Entitie;
using Unity.Test.Utils.Repository.User;
using Unity.Test.Utils.Token;
using Xunit;

namespace UseCase.Test.Login.DoLogin;

public class LoginUser
{
    [Fact]
    public async Task Validate_Success()
    {
        (var user, var pass) = UserBuilder.Build();

        var useCase = CreateUseCase(user);
        var response = await useCase.Execute(new RequestLoginJson
        {
            Email = user.Email,
            Password = pass
        });

        response.Should().NotBeNull();
        response.Name.Should().Be(user.Name);
        response.Token.Should().NotBeNullOrWhiteSpace();
    }
    [Fact]
    public async Task Validate_Invalid_Pass_Erro()
    {
        (var user, var pass) = UserBuilder.Build();

        var useCase = CreateUseCase(user);
        Func<Task> action = async () =>
        {
            await useCase.Execute(new RequestLoginJson
            {
                Email = user.Email,
                Password = "passInvalid"
            });
        };

        await action.Should().ThrowAsync<InvalidLoginException>()
            .Where(exception => exception.Message.Equals(ErroMessagesResource.User_Login_Invalid));
    }
    [Fact]
    public async Task Validate_Email_Invalid_Erro()
    {
        (var user, var pass) = UserBuilder.Build();

        var useCase = CreateUseCase(user);
        Func<Task> action = async () =>
        {
            await useCase.Execute(new RequestLoginJson
            {
                Email = "email@invalid.com",
                Password = pass
            });
        };

        await action.Should().ThrowAsync<InvalidLoginException>()
            .Where(exception => exception.Message.Equals(ErroMessagesResource.User_Login_Invalid));
    }
    [Fact]
    public async Task Validate_EmailPass_Invalid_Erro()
    {
        (var user, var pass) = UserBuilder.Build();

        var useCase = CreateUseCase(user);
        Func<Task> action = async () =>
        {
            await useCase.Execute(new RequestLoginJson
            {
                Email = "email@invalid.com",
                Password = "passInvalid"
            });
        };

        await action.Should().ThrowAsync<InvalidLoginException>()
            .Where(exception => exception.Message.Equals(ErroMessagesResource.User_Login_Invalid));
    }

    private static MyRecipeBook.Application.UseCases.Login.DoLogin.LoginUserUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var encPassword = EncPasswordBuilder.Instance();
        var token = TokenControllerBuilder.Instance();
        var readRepository = UserReadOnlyRepositoryBuilder.Instance().RecorverByEmailPass(user).Build();

        return new MyRecipeBook.Application.UseCases.Login.DoLogin.LoginUserUseCase(readRepository, encPassword, token);
    }
}
