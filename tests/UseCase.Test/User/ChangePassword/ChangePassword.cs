using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Exceptions.BaseException;
using MyRecipeBook.Exceptions;
using Unity.Test.Utils.Cryptography;
using Unity.Test.Utils.Entitie;
using Unity.Test.Utils.Repository.User;
using Unity.Test.Utils.Repository;
using Unity.Test.Utils.Requests;
using Unity.Test.Utils.UserLogged;
using Xunit;

namespace UseCase.Test.User.ChangePassword;

public class ChangePassword
{
    [Fact]
    public async Task Validate_Success()
    {
        (var user, var pass) = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var request = RequestChangePasswordBuilder.Build();
        request.ActualPassword = pass;

        Func<Task> response = async () =>
        {
            await useCase.Execute(request);
        };

        await response.Should().NotThrowAsync();
    }
    [Fact]
    public async Task Validate_New_Password_Empty_Erro()
    {
        (var user, var pass) = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        Func<Task> response = async () =>
        {
            await useCase.Execute(new RequestChangePasswordJson
            {
                ActualPassword = pass,
                NewPassword = ""
            });
        };

        await response.Should().ThrowAsync<ValidationErroException>()
            .Where(e => e.ErroMessages.Count == 1 && e.ErroMessages.Contains(ErroMessagesResource.User_Password_Empty));
    }
    [Fact]
    public async Task Validate_Actual_Password_On_Token_Erro()
    {
        (var user, var pass) = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var request = RequestChangePasswordBuilder.Build();
        request.ActualPassword = "invalidPassword";

        Func<Task> response = async () =>
        {
            await useCase.Execute(request);
        };

        await response.Should().ThrowAsync<ValidationErroException>()
            .Where(e => e.ErroMessages.Count == 1 && e.ErroMessages.Contains(ErroMessagesResource.User_Password_Invalid));
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Validate_Actual_Password_On_Token_Min_Char_Erro(int passwordLength)
    {
        (var user, var pass) = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var request = RequestChangePasswordBuilder.Build(passwordLength);
        request.ActualPassword = pass;

        Func<Task> response = async () =>
        {
            await useCase.Execute(request);
        };

        await response.Should().ThrowAsync<ValidationErroException>()
            .Where(e => e.ErroMessages.Count == 1 && e.ErroMessages.Contains(ErroMessagesResource.User_Password_MinChar));
    }

    private static ChangePasswordUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var update = UserUpdateOnlyRepositoryBuilder.Instance().RecoverById(user).Build();
        var encPassword = EncPasswordBuilder.Instance();
        var unityOfWork = UnityOfWorkBuilder.Instance().Build();
        var userLogged = UserLoggedBuilder.Instance().RecoverUser(user).Build();

        return new ChangePasswordUseCase(update, userLogged, unityOfWork, encPassword);
    }
}
