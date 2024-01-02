using FluentAssertions;
using MyRecipeBook.Api;
using MyRecipeBook.Communication.Enum;
using MyRecipeBook.Exceptions;
using System.Net;
using System.Text.Json;
using Unity.Test.Utils.Requests;
using Xunit;

namespace Web.Api.Test.V1.Recipe.Registry;

public class RegistryRecipe : ControllerBase
{
    private const string Method = "recipe";
    private readonly MyRecipeBook.Domain.Entities.User _user;
    private readonly string _pass;

    public RegistryRecipe(MyRecipeBookWebFactory<Program> factory) : base(factory)
    {
        _user = factory.RecoverUser();
        _pass = factory.RecoverPass();
    }

    [Fact]
    public async Task Validate_Success()
    {
        var token = await Login(_user.Email, _pass);

        var request = RequestRegistryRecipeBuilder.Build();

        var response = await PostRequest(Method, request, token);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responstaBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        responseData.RootElement.GetProperty("id").GetString().Should().NotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
        responseData.RootElement.GetProperty("category").GetUInt16().Should().Be((ushort)request.Category);
        responseData.RootElement.GetProperty("preparationMethod").GetString().Should().Be(request.PreparationMethod);
    }
    [Fact]
    public async Task Validate_Recipe_Title_Empty_Erro()
    {
        var token = await Login(_user.Email, _pass);

        var request = RequestRegistryRecipeBuilder.Build();
        request.Title = string.Empty;

        var response = await PostRequest(Method, request, token);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("erroMessages").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ErroMessagesResource.User_Recipe_Title_Empty));
    }
    [Fact]
    public async Task Validate_Recipe_Category_Empty_Erro()
    {
        var token = await Login(_user.Email, _pass);

        var request = RequestRegistryRecipeBuilder.Build();
        request.Category = (Category)9999;

        var response = await PostRequest(Method, request, token);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("erroMessages").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ErroMessagesResource.User_Recipe_Category_Invalid));
    }
    [Fact]
    public async Task Validate_Recipe_Preparation_Method_Empty_Erro()
    {
        var token = await Login(_user.Email, _pass);

        var request = RequestRegistryRecipeBuilder.Build();
        request.PreparationMethod = string.Empty;

        var response = await PostRequest(Method, request, token);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("erroMessages").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ErroMessagesResource.User_Recipe_Preparation_Method_Empty));
    }
    [Fact]
    public async Task Validate_Recipe_Ingredients_Empty_Erro()
    {
        var token = await Login(_user.Email, _pass);

        var request = RequestRegistryRecipeBuilder.Build();
        request.Ingredients.Clear();

        var response = await PostRequest(Method, request, token);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("erroMessages").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ErroMessagesResource.User_Recipe_Ingredients_Empty));
    }
    [Fact]
    public async Task Validate_Recipe_Ingredients_Product_Empty_Erro()
    {
        var token = await Login(_user.Email, _pass);

        var request = RequestRegistryRecipeBuilder.Build();
        request.Ingredients.First().Product = string.Empty;

        var response = await PostRequest(Method, request, token);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("erroMessages").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ErroMessagesResource.User_Recipe_Ingredients_Product_Empty));
    }
    [Fact]
    public async Task Validate_Recipe_Ingredients_Quantity_Empty_Erro()
    {
        var token = await Login(_user.Email, _pass);

        var request = RequestRegistryRecipeBuilder.Build();
        request.Ingredients.First().Quantity = string.Empty;

        var response = await PostRequest(Method, request, token);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("erroMessages").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ErroMessagesResource.User_Recipe_Ingredients_Quantity_Empty));
    }
}
