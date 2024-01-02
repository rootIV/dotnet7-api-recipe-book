using FluentAssertions;
using MyRecipeBook.Api;
using System.Net;
using System.Text.Json;
using Unity.Test.Utils.HashIds;
using Xunit;

namespace Web.Api.Test.V1.Connection;

public class RemoveConnections : ControllerBase
{
    private const string Method = "connections";
    private readonly MyRecipeBook.Domain.Entities.User _userWithoutConnection;
    private readonly string _passWithoutConnection;
    private readonly MyRecipeBook.Domain.Entities.User _userWithConnection;
    private readonly string _passWithConnection;

    public RemoveConnections(MyRecipeBookWebFactory<Program> factory) : base(factory)
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

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var users = responseData.RootElement.GetProperty("users").EnumerateArray();

        var connectedUserIdToRemove = users.First().GetProperty("id").GetString();

        var responseDelete = await DeleteRequest($"{Method}/{connectedUserIdToRemove}", token);
        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var responseGetAfterDelete = await GetRequest(Method, token);
        responseGetAfterDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    [Fact]
    public async Task Validate_User_Invalid_Id_Erro()
    {
        var token = await Login(_userWithConnection.Email, _passWithConnection);

        var connectedUserIdToRemove = HashIdsBuilder.Instance().Build().EncodeLong(0);

        var response = await DeleteRequest($"{Method}/{connectedUserIdToRemove}", token);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task Validate_User_Without_Connection_Erro()
    {
        var token = await Login(_userWithoutConnection.Email, _passWithoutConnection);

        var connectedUserIdToRemove = HashIdsBuilder.Instance().Build().EncodeLong(0);

        var response = await DeleteRequest($"{Method}/{connectedUserIdToRemove}", token);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
