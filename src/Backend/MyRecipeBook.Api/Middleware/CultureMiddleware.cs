using System.Globalization;

namespace MyRecipeBook.Api.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;
    private readonly List<string> _supportedLanguages = new()
    {
        "pt",
        "en"
    };

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var culture = new CultureInfo("en");

        if (context.Request.Headers.ContainsKey("Accept-Language"))
        {
            var language = context.Request.Headers["Accept-Language"].ToString();

            if (_supportedLanguages.Contains(language))
                culture = new CultureInfo(language);
        }

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        await _next(context);
    }
}
