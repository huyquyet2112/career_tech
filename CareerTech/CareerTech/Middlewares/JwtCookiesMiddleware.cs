namespace CareerTech.Middlewares;

public class JwtCookiesMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Cookies.ContainsKey("tokenCT"))
        {
            var token = context.Request.Cookies["tokenCT"];
            if(!string.IsNullOrEmpty(token))
            {
                context.Request.Headers.Append("Authorization", "Bearer " + token);
            }
        }

        await _next(context);
    }
}
