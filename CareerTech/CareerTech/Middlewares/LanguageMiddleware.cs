using System.Globalization;

namespace CareerTech.Middlewares;

public class LanguageMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.Cookies.TryGetValue("lang", out string? language);
        language ??= "en";
        Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
        await _next(context);
    }
}
