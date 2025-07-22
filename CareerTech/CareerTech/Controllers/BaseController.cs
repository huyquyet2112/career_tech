using CareerTech.Model.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CareerTech.Controllers;

public class BaseController(IStringLocalizer<Language> localizer): Controller
{
    protected IStringLocalizer Localizer { get; set; } = localizer;

    protected UserInfo UserInfo { get; set; }

    protected ViewResult ViewWithModel(object vm, object? fm = null)
    {
        SetViewModel(vm);
        return View(fm);
    }

    protected ViewResult ViewWithModel(string viewName, object vm, object? fm = null)
    {
        SetViewModel(vm);
        return View(viewName, fm);
    }

    private void SetViewModel(object vm)
    {
        ViewBag._ViewModel = vm;
    }

    public int GetUserId()
    {
        if (!(this.HttpContext?.User?.Identity?.IsAuthenticated ?? false))
        {
            return 0;
        }

        if (this.UserInfo == default)
        {
            this.UserInfo = new UserInfo(this.User);
        }

        return this.UserInfo.Id;
    }

    public string? GetUserRole()
    {
        if (!(this.HttpContext?.User?.Identity?.IsAuthenticated ?? false))
        {
            return null;
        }

        if (this.UserInfo == default)
        {
            this.UserInfo = new UserInfo(this.User);
        }

        return this.UserInfo.Role;
    }

}
