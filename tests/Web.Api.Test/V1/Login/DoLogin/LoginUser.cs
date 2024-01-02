using FluentAssertions;
using MyRecipeBook.Api;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Exceptions;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Web.Api.Test.V1.Login.DoLogin;

public class LoginUser : ControllerBase
{
    private const string Method = "login";
    private readonly MyRecipeBook.Domain.Entities.User _user;
    private readonly string _pass;

    public LoginUser(MyRecipeBookWebFactory<Program> factory) : base(factory)
    {
        _user = factory.RecoverUser();
        _pass = factory.RecoverPass();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var request = new RequestLoginJson
        {
            Email = _user.Email,
            Password = _pass
        };

        var response = await PostRequest(Method, request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_user.Name);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }
    [Fact]
    public async Task Validate_Invalid_Email_Erro()
    {
        var request = new RequestLoginJson
        {
            Email = "email@invalid.com",
            Password = _pass
        };

        var response = await PostRequest(Method, request);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("erroMessages").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ErroMessagesResource.User_Login_Invalid));
    }
    [Fact]
    public async Task Validate_Invalid_Pass_Erro()
    {
        var request = new RequestLoginJson
        {
            Email = _user.Email,
            Password = "passInvalid"
        };

        var response = await PostRequest(Method, request);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("erroMessages").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ErroMessagesResource.User_Login_Invalid));
    }
    [Fact]
    public async Task Validate_Invalid_EmailPass_Erro()
    {
        var request = new RequestLoginJson
        {
            Email = "email@invalid.com",
            Password = "passInvalid"
        };

        var response = await PostRequest(Method, request);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("erroMessages").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ErroMessagesResource.User_Login_Invalid));
    }
}
