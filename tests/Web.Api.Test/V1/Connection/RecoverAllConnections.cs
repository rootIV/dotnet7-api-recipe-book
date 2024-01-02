using FluentAssertions;
using MyRecipeBook.Api;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Web.Api.Test.V1.Connection;

public class RecoverAllConnections : ControllerBase
{
    private const string Method = "connections";
    private readonly MyRecipeBook.Domain.Entities.User _userWithoutConnection;
    private readonly string _passWithoutConnection;
    private readonly MyRecipeBook.Domain.Entities.User _userWithConnection;
    private readonly string _passWithConnection;


    public RecoverAllConnections(MyRecipeBookWebFactory<Program> factory) : base(factory)
    {
        _userWithoutConnection = factory.RecoverUser();
        _passWithoutConnection = factory.RecoverPass();
        _userWithConnection = factory.RecoverUserWithConnection();
        _passWithConnection = factory.RecoverPassUserWithConnection();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var token = await Login(_userWithConnection.Email, _passWithConnection);

        var response = await GetRequest(Method, token);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
        responseData.RootElement.GetProperty("users").GetArrayLength().Should().BeGreaterThan(0);
    }
    [Fact]
    public async Task Validate_User_Without_Connection()
    {
        var token = await Login(_userWithoutConnection.Email, _passWithoutConnection);

        var response = await GetRequest(Method, token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
