using FluentAssertions;
using MyRecipeBook.Api;
using MyRecipeBook.Exceptions;
using System.Net;
using System.Text.Json;
using Unity.Test.Utils.Requests;
using Xunit;

namespace Web.Api.Test.V1.User.ChangePassword;

public class ChangePassword : ControllerBase
{
    private const string Method = "user/change-password";
    private readonly MyRecipeBook.Domain.Entities.User _user;
    private readonly string _pass;

    public ChangePassword(MyRecipeBookWebFactory<Program> factory) : base(factory)
    {
        _user = factory.RecoverUser();
        _pass = factory.RecoverPass();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var token = await Login(_user.Email, _pass);

        var request = RequestChangePasswordBuilder.Build();
        request.ActualPassword = _pass;

        var response = await PutRequest(Method, request, token);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    [Fact]
    public async Task Validate_Password_Empty_Erro()
    {
        var token = await Login(_user.Email, _pass);

        var request = RequestChangePasswordBuilder.Build();
        request.ActualPassword = _pass;
        request.NewPassword = string.Empty;

        var response = await PutRequest(Method, request, token);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("erroMessages").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(e => e.GetString().Contains(ErroMessagesResource.User_Password_Empty));
    }
}
