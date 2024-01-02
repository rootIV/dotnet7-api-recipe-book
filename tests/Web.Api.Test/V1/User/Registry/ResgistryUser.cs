using FluentAssertions;
using MyRecipeBook.Api;
using MyRecipeBook.Exceptions;
using System.Net;
using System.Text.Json;
using Unity.Test.Utils.Requests;
using Xunit;

namespace Web.Api.Test.V1.User.Registry;

public class ResgistryUser : ControllerBase
{
    private const string Method = "user";

    public ResgistryUser(MyRecipeBookWebFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task Validate_Success()
    {
        var request = RequestRegistryUserBuilder.Build();

        var response = await PostRequest(Method, request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Validate_Erro_Empty_Name()
    {
        var request = RequestRegistryUserBuilder.Build();
        request.Name = "";

        var response = await PostRequest(Method, request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("erroMessages").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(e => e.GetString().Equals(ErroMessagesResource.User_Name_Empty));
    }
}
