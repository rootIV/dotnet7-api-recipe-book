using MyRecipeBook.Api;
using MyRecipeBook.Communication.Request;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Web.Api.Test.V1;

public class ControllerBase : IClassFixture<MyRecipeBookWebFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public ControllerBase(MyRecipeBookWebFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }

    protected async Task<HttpResponseMessage> PostRequest(string method, object body, string token = "", string culture = "")
    {
        AuthorizeRequest(token);
        ChangeCultureRequest(culture);

        var jsonString = JsonConvert.SerializeObject(body);

        return await _httpClient.PostAsync(method, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }
    protected async Task<HttpResponseMessage> GetRequest(string method, string token = "", string culture = "")
    {
        AuthorizeRequest(token);
        ChangeCultureRequest(culture);

        return await _httpClient.GetAsync(method);
    }
    protected async Task<HttpResponseMessage> DeleteRequest(string method, string token = "", string culture = "")
    {
        AuthorizeRequest(token);
        ChangeCultureRequest(culture);

        return await _httpClient.DeleteAsync(method);
    }
    protected async Task<HttpResponseMessage> PutRequest(string method, object body, string token = "", string culture = "")
    {
        AuthorizeRequest(token);
        ChangeCultureRequest(culture);

        var jsonString = JsonConvert.SerializeObject(body);

        return await _httpClient.PutAsync(method, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }
    protected async Task<string> Login(string email, string password)
    {
        var request = new RequestLoginJson
        {
            Email = email,
            Password = password
        };

        var response = await PostRequest("login", request);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        return responseData.RootElement.GetProperty("token").GetString();
    }
    protected async Task<string> GetRecipeId(string token)
    {
        var request = new RequestDashboardJson();

        var response = await PutRequest("dashboard", request, token);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        return responseData.RootElement.GetProperty("recipes").EnumerateArray().First().GetProperty("id").GetString();
    }

    private void AuthorizeRequest(string token)
    {
        _httpClient.DefaultRequestHeaders.Remove("Authorization");

        if (!string.IsNullOrWhiteSpace(token))
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
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
