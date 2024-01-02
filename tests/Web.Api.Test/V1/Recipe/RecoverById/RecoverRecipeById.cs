using FluentAssertions;
using MyRecipeBook.Api;
using System.Net;
using System.Text.Json;
using Unity.Test.Utils.Requests;
using Xunit;

namespace Web.Api.Test.V1.Recipe.RecoverById;

public class RecoverRecipeById : ControllerBase
{
    private const string Method = "recipe";
    private readonly MyRecipeBook.Domain.Entities.User _user;
    private readonly string _pass;

    public RecoverRecipeById(MyRecipeBookWebFactory<Program> factory) : base(factory)
    {
        _user = factory.RecoverUser();
        _pass = factory.RecoverPass();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var token = await Login(_user.Email, _pass);

        var recipeId = await GetRecipeId(token);

        var response = await PutRequest($"{Method}/{recipeId}", token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetString().Should().NotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("category").GetUInt16().Should().BeInRange(0, 3);
        responseData.RootElement.GetProperty("preparationMethod").GetString().Should().NotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("ingredients").GetArrayLength().Should().BeGreaterThan(0);
        responseData.RootElement.GetProperty("preparationTime").GetUInt32().Should().BeGreaterThan(0).And.BeLessThanOrEqualTo(1000);
    }
}
