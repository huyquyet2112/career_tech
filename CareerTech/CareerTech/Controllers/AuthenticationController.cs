using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using CareerTech.Shared;
using CareerTech.Request.Authentication;
using CareerTech.Service.Interfaces;
using CareerTech.Model.Entities;
using System.Text;
using CareerTech.Model.Enums;
using System.Security.Claims;

namespace CareerTech.Controllers;

public class AuthenticationController(
    ILogger<AuthenticationController> logger,
    IStringLocalizer<Language> localizer,
    IAuthenticationService authenticationService
    ) : BaseController(localizer)
{
    private readonly ILogger<AuthenticationController> logger = logger;
    private readonly IAuthenticationService authenticationService = authenticationService;

    [HttpGet]
    [Route("login/admin")]
    [AllowAnonymous]
    public IActionResult LoginAdmin()
    {
        var token = this.Request.Cookies["tokenCT"];
        if (token != default)
        {
            var userInfo = this.authenticationService.ValidateToken(token);
            if (userInfo != null)
            {
                if (userInfo.Role == EUserRole.Admin.ToString())
                {
                    return this.Redirect("/admin/dashboard");
                }
            }
        }
        return this.View(ViewMapping.ViewAdminLogin);
    }


    [HttpGet]
    [Route("register")]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return this.View(ViewMapping.ViewRegister);
    }

    [HttpGet]
    [Route("login")]
    [AllowAnonymous]
    public IActionResult Login()
    {
        var token = this.Request.Cookies["tokenCT"];
        if (token != default)
        {
            var userInfo = this.authenticationService.ValidateToken(token);
            if (userInfo != null)
            {
                if (userInfo.Role == EUserRole.Applicant.ToString())
                {
                    return this.Redirect("/applicants");
                }
                else
                {
                    this.HttpContext.Response.Cookies.Delete("tokenCT");
                }
            }
        }
        return this.View(ViewMapping.ViewApplicantLogin);
    }

    
    [HttpGet]
    [Route("login/recruiter")]
    [AllowAnonymous]
    public IActionResult LoginRecruiter()
    {
        var token = this.Request.Cookies["tokenCT"];
        if (token != default)
        {
            var userInfo = this.authenticationService.ValidateToken(token);
            if (userInfo != null)
            {
                if (userInfo.Role == EUserRole.Recruitment.ToString())
                {
                    return this.Redirect("/recruiters/dashboard");
                }
                else
                {
                    this.HttpContext.Response.Cookies.Delete("tokenCT");
                }
            }
        }
        return this.View(ViewMapping.ViewRecruiterLogin);
    }

    
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> SubmitLogin([FromForm] LoginDto loginDto)
    {
        this.ViewData["user"] = loginDto;

        try
        {
            var token = await this.authenticationService.Login(loginDto);
            var expiredTime = DateTime.Now.AddDays(AppSettingProvider.AppSettings.Jwt.ExpiredTime);
            if (loginDto.StayLogin == default)

            {
                expiredTime = DateTime.Now.AddDays(1);
            }

            CookieOptions option = new()
            {
                Expires = expiredTime,
            };

            this.HttpContext.Response.Cookies.Append("tokenCT", token, option);

            if (!string.IsNullOrEmpty(loginDto.ReturnUrl))
            {
                return Redirect(loginDto.ReturnUrl);
            }

            return loginDto.Role switch
            {
                EUserRole.Applicant => Redirect("/applicants/"),
                EUserRole.Admin => Redirect("/admin/dashboard"),
                EUserRole.Recruitment => Redirect("/recruiters/dashboard"),
                _ => throw new NotImplementedException(),
            };
        }
        catch (Exception ex)
        {
            this.ViewBag.error = Utils.GetLocalizedMessage(ex.Message, this.Localizer);
            this.ViewBag.returnUrl = loginDto.ReturnUrl;
            return loginDto.Role switch
            {
                EUserRole.Applicant => this.View(ViewMapping.ViewApplicantLogin),
                EUserRole.Admin => this.View(ViewMapping.ViewAdminLogin),
                EUserRole.Recruitment => this.View(ViewMapping.ViewRecruiterLogin),
                _ => throw new NotImplementedException(),
            };
        }
    }

    
    [Route("logout")]
    [AllowAnonymous]
    public IActionResult Logout()
    {
        this.HttpContext.Response.Cookies.Delete("tokenCT");
        return this.Redirect("/login");
    }

    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    public async Task<IActionResult> SubmitRegister([FromForm] RegisterDto registerDto)
    {
        this.ViewData["user"] = registerDto;

        try
        {
            var result = await this.authenticationService.Register(registerDto);

            if (result)
            {
                return Redirect("/login");
            }
            else
            {
                this.ViewBag.error = Utils.GetLocalizedMessage("errRegisterFailed", this.Localizer);
                return this.View(ViewMapping.ViewRegister);
            }
        }
        catch(Exception ex)
        {
            this.ViewBag.error = Utils.GetLocalizedMessage(ex.Message, this.Localizer);
            return this.View(ViewMapping.ViewRegister);
        }
    }
}
