using FluentAssertions;
using MyRecipeBook.Api;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Exceptions;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Web.Api.Test.V1.UnknownErro;

public class LoginUnknownErro : IClassFixture<MyRecipeBookWebFactoryWithouLoginDependecyInjection<Program>>
{
    private const string Method = "login";
    private readonly HttpClient _httpClient;

    public LoginUnknownErro(MyRecipeBookWebFactoryWithouLoginDependecyInjection<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Theory]
    [InlineData("pt")]
    [InlineData("en")]
    public async Task Validate_Unknown_Erro(string culture)
    {
        var request = new RequestLoginJson();

        var response = await PostRequest(Method, request, culture);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("erroMessages").EnumerateArray();

        var waitedMessage = ErroMessagesResource.ResourceManager.GetString("UNKNOWN_ERRO", new System.Globalization.CultureInfo(culture));

        erros.Should().HaveCountGreaterThan(0).And.Contain(e => e.GetString().Equals(waitedMessage));
    }

    private async Task<HttpResponseMessage> PostRequest(string method, object body, string culture)
    {
        ChangeCultureRequest(culture);

        var jsonString = JsonConvert.SerializeObject(body);

        return await _httpClient.PostAsync(method, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }
    private void ChangeCultureRequest(string culture)
    {
        if (!string.IsNullOrWhiteSpace(culture))
        {
            if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
            }
        }

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
    }
}
