using FluentAssertions;
using MyRecipeBook.Api;
using System.Net;
using Unity.Test.Utils.Requests;
using Unity.Test.Utils.Token;
using Xunit;

namespace Web.Api.Test.V1.User.ChangePassword;

public class ChangePasswordInvalidToken : ControllerBase
{
    private const string Method = "user/change-password";
    private readonly MyRecipeBook.Domain.Entities.User _user;
    private readonly string _pass;

    public ChangePasswordInvalidToken(MyRecipeBookWebFactory<Program> factory) : base(factory)
    {
        _user = factory.RecoverUser();
        _pass = factory.RecoverPass();
    }

    [Fact]
    public async Task Validate_Token_Empty_Erro()
    {
        var token = string.Empty;

        var request = RequestChangePasswordBuilder.Build();
        request.ActualPassword = _pass;

        var response = await PutRequest(Method, request, token);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    [Fact]
    public async Task Validate_Token_Fake_User_Erro()
    {
        var token = TokenControllerBuilder.Instance().GenerateToken("fake@user.com");

        var request = RequestChangePasswordBuilder.Build();
        request.ActualPassword = _pass;

        var response = await PutRequest(Method, request, token);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    [Fact]
    public async Task Validate_Token_Expired_Erro()
    {
        var token = TokenControllerBuilder.ExpiredToken().GenerateToken(_user.Email);
        await Task.Delay(1000);

        var request = RequestChangePasswordBuilder.Build();
        request.ActualPassword = _pass;

        var response = await PutRequest(Method, request, token);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
