using CareerTech.Common.Utils;
using CareerTech.Model.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace CareerTech.Attributes;

public class PermissionRequireHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IList<string> AutoAcceptAction = new List<string>()
    {
        Helper.Encode("Authentication.Login"),
        Helper.Encode("Authentication.LoginAdmin"),
        Helper.Encode("Authentication.LoginRecruiter"),
        Helper.Encode("Authentication.SubmitLogin"),
        Helper.Encode("Authentication.Register"),
        Helper.Encode("Authentication.Logout"),
        Helper.Encode("Authentication.SubmitRegister"),
        Helper.Encode("Applicant.Index"),
        Helper.Encode("Applicant.GetPublicJobs"),
        Helper.Encode("Applicant.GetDetailJobPublic"),
        Helper.Encode("Applicant.GetPublicRecruiter"),
        Helper.Encode("Applicant.GetProfileRecruiterPublic"),
        Helper.Encode("ApiUser.ForgotPassword"),
        Helper.Encode("ApiUser.VerificationCode"),
        Helper.Encode("ApiUser.ResetPassword"),
        Helper.Encode("Home.Index"),
    };

    private readonly List<string> AutoAcceptFolder = new List<string>() {
        "/images/",
    };

    public PermissionRequireHandler(IHttpContextAccessor _httpContextAccessor)
    {
        httpContextAccessor = _httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (httpContextAccessor.HttpContext != null)
        {
            var requestPath = httpContextAccessor.HttpContext.Request.Path.Value;
            if (requestPath.StartsWith("/images/", StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
        }

        if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
        {
            var controllerName = httpContextAccessor.HttpContext.GetRouteValue("controller")?.ToString();
            var actionName = httpContextAccessor.HttpContext.GetRouteValue("action")?.ToString();
            var controllerActionName = Helper.Encode($"{controllerName}.{actionName}");

            if (this.AutoAcceptAction.Contains(controllerActionName))
            {
                context.Succeed(requirement);
            }
            else
            {
                var userInfo = new UserInfo(context.User.Claims);
                if (userInfo.Permissions.Contains(Helper.Encode($"{controllerName}.{actionName}")))
                {
                    context.Succeed(requirement);
                }
            }
        }

        return Task.CompletedTask;
    }
}
