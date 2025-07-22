using CareerTech.Model.Dtos;
using CareerTech.Request.Languages;
using CareerTech.Response.Apis;
using CareerTech.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Globalization;

namespace CareerTech.Controllers;

public class HomeController :  BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly IStringLocalizer<Language> _localizer;
    private readonly IActionDescriptorCollectionProvider _provider;

    public HomeController(
        ILogger<HomeController> logger,
        IStringLocalizer<Language> localizer,
        IActionDescriptorCollectionProvider provider) : base(localizer) 
    {
        _logger = logger;
        _localizer = localizer;
        _provider = provider;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return this.Redirect("/applicants");
    }

    [Route("403")]
    [AllowAnonymous]
    public IActionResult ForbiddenError()
    {
        return View(ViewMapping.ViewForbiddenError);
    }

    [Route("error")]
    [AllowAnonymous]
    public IActionResult Error()
    {
        return HttpContext.Response.StatusCode switch
        {
            StatusCodes.Status403Forbidden => View(ViewMapping.ViewForbiddenError),
            StatusCodes.Status404NotFound => View(ViewMapping.ViewNotFoundError),
            _ => View(ViewMapping.ViewInternalServerError),
        };
    }

    [Route("language")]
    [AllowAnonymous]
    public IActionResult ChangeLanguage([FromQuery] ChangeLanguageDto requestDto)
    {
        var language = requestDto.Lang;
        var languageSupport = AppSettingProvider.AppSettings.LanguageSupports;

        if (!languageSupport.Any(l => l.Code == language))
        {
            return BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("errLanguageNotSupport", Localizer)));
        }

        Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
        Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
        Response.Cookies.Append("lang", language);
        return Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgChangeLanguageSuccess", Localizer)));
    }
}
