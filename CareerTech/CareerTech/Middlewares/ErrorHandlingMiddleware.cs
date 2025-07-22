using CareerTech.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CareerTech.Middlewares;

public class ErrorHandlingMiddleware(
  RequestDelegate next,
  ILogger<ErrorHandlingMiddleware> logger,
  IRazorViewEngine viewEngine,
  ITempDataDictionaryFactory tempDataDictionaryFactory
)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;
    private readonly IRazorViewEngine _viewEngine = viewEngine;
    private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory = tempDataDictionaryFactory;

    public async Task InvokeAsync(HttpContext context)
    {
        if (IsApiRequest(context))
        {
            await _next(context);
            return;
        }
        try
        {
            await _next(context);
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                // Handle 404 error
                await RenderViewAsync(context, ViewMapping.ViewNotFoundError);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            // Handle exceptions
            await RenderViewAsync(context, ViewMapping.ViewInternalServerError, ex);
        }
    }

    private async Task RenderViewAsync(HttpContext context, string viewName, Exception? exception = default)
    {
        context.Response.StatusCode = exception != default
            ? StatusCodes.Status500InternalServerError
            : StatusCodes.Status404NotFound;
        context.Response.ContentType = "text/html";

        using var writer = new StringWriter();
        var actionContext = new ActionContext(context, new RouteData(), new ActionDescriptor());

        var viewResult = _viewEngine.GetView("", viewName, true);

        if (!viewResult.Success)
        {
            throw new InvalidOperationException($"View '{viewName}' not found.");
        }

        var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            Model = exception
        };

        var tempData = _tempDataDictionaryFactory.GetTempData(context);

        var viewContext = new ViewContext(
            actionContext,
            viewResult.View,
            viewDictionary,
            tempData,
            writer,
            new HtmlHelperOptions()
        );

        await viewResult.View.RenderAsync(viewContext);
        var renderedView = writer.GetStringBuilder().ToString();

        await context.Response.WriteAsync(renderedView);
    }

    private bool IsApiRequest(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/api") ||
               context.Request.Headers.Accept.Any(value => value?.Contains("application/json") ?? false);
    }
}
