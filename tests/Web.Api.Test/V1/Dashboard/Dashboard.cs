using FluentAssertions;
using MyRecipeBook.Api;
using MyRecipeBook.Communication.Request;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Web.Api.Test.V1.Dashboard;

public class Dashboard : ControllerBase
{
    private const string Method = "dashboard";
    private readonly MyRecipeBook.Domain.Entities.User _userWithRecipe;
    private readonly string _passUserWithRecipe;
    private readonly MyRecipeBook.Domain.Entities.User _userWithoutRecipe;
    private readonly string _passUserWithoutRecipe;

    public Dashboard(MyRecipeBookWebFactory<Program> factory) : base(factory)
    {
        _userWithRecipe = factory.RecoverUser();
        _passUserWithRecipe = factory.RecoverPass();
        _userWithoutRecipe = factory.RecoverUserWithoutRecipe();
        _passUserWithoutRecipe = factory.RecoverPassUserWithoutRecipe();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var token = await Login(_userWithRecipe.Email, _passUserWithRecipe);

        var response = await PutRequest(Method, new RequestDashboardJson(), token);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("recipes").GetArrayLength().Should().BeGreaterThan(0);
    }
    [Fact]
    public async Task Validate_Recipe_Not_Found_Erro()
    {
        var token = await Login(_userWithRecipe.Email, _passUserWithRecipe);

        var response = await PutRequest(Method, new RequestDashboardJson(), token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
