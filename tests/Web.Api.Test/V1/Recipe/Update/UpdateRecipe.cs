using FluentAssertions;
using MyRecipeBook.Api;
using Unity.Test.Utils.Requests;
using System.Net;
using Xunit;

namespace Web.Api.Test.V1.Recipe.Update;

public class UpdateRecipe : ControllerBase
{
    private const string Method = "recipe";
    private readonly MyRecipeBook.Domain.Entities.User _user;
    private readonly string _pass;

    public UpdateRecipe(MyRecipeBookWebFactory<Program> factory) : base(factory)
    {
        _user = factory.RecoverUser();
        _pass = factory.RecoverPass();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var token = await Login(_user.Email, _pass);

        var request = RequestRegistryRecipeBuilder.Build();

        var recipeId = await GetRecipeId(token);

        var response = await PutRequest($"{Method}/{recipeId}", request, token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        //var responseData = await GetRecipeById();
    }
}
