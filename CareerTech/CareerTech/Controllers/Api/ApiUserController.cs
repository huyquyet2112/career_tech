using CareerTech.Model.Dtos;
using CareerTech.Request.Errors;
using CareerTech.Request.Users;
using CareerTech.Response.Apis;
using CareerTech.Service.Interfaces;
using CareerTech.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;


namespace CareerTech.Controllers.Api;

[Route("api/users")]
public class ApiUserController : BaseController
{
    private readonly IStringLocalizer<Language> localizer;
    private readonly IUserService userService;
    private readonly IEmailService emailService;

    public ApiUserController(
        IStringLocalizer<Language> localizer,
        IUserService userService,
        IEmailService emailService) : base(localizer)
    {
        this.localizer = localizer;
        this.userService = userService;
        this.emailService = emailService;
    }

    [Route("{userId}/change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordUserDto requestDto, int userId)
    {
        try
        {
            var validate = requestDto.Validate();

            if (validate.Count != 0)
            {
                var errorResponse = new List<ErrorValidateDto>();

                foreach (var error in validate)
                {
                    var message = new MessageResponseDto(Utils.GetLocalizedMessage($"{error.Error}", this.Localizer));
                    errorResponse.Add(new ErrorValidateDto
                    {
                        Name = error.Name,
                        Error = message.Message,
                    });
                }

                return this.BadRequest(errorResponse);
            }

            var result = await this.userService.ChangePassword(requestDto, userId);

            if (result)
            {
                if (this.IsThisUserChange(userId))
                {
                    return this.Ok(
                    new
                    {
                        Message = new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdatePasswordSuccess", this.localizer)),
                        shouldLogout = true,
                    });
                }

                else
                {
                    return this.Ok(
                    new
                    {
                        Message = new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdatePasswordSuccess", this.localizer)),
                        shouldLogout = false,
                    });
                }

            }
            else
            {
                return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdatePasswordFailed", this.localizer)));
            }
        }
        catch (Exception ex)
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage($"{ex}", this.localizer)));
        }
    }

    private bool IsThisUserChange(int userId)
    {
        var userLoggedIn = new UserInfo(this.Request.HttpContext.User);
        return userLoggedIn.CheckIsThisUserLoggedIn(userId);
    }

    [HttpPost]
    [Route("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] EmailForgotPasswordDto requestDto)
    {
        var result = await this.userService.GenerateForgotPassword(requestDto);

        if (result.Success)
        {
            return Ok(new
            {
                result.Success,
                result.Email,
            });
        }

        return BadRequest(new
        {
            result.Success,
            Message = new MessageResponseDto(Utils.GetLocalizedMessage($"{result.Message}", this.Localizer))
        });
    }

    [HttpPost]
    [Route("verification-code")]
    [AllowAnonymous]
    public async Task<IActionResult> VerificationCode([FromBody] ConfirmVerificationCodeDto requestDto)
    {
        var result = await this.userService.ConfirmVerificationCode(requestDto);

        if (result.Success)
        {
            return Ok(new
            {
                result.Success,
                result.Email,
            });
        }

        return BadRequest(new
        {
            result.Success,
            Message = new MessageResponseDto(Utils.GetLocalizedMessage($"{result.Message}", this.Localizer))
        });
    }

    [HttpPut]
    [Route("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto requestDto)
    {
        var validate = requestDto.Validate();

        if (validate.Count != 0)
        {
            var errorResponse = new List<ErrorValidateDto>();

            foreach (var error in validate)
            {
                var message = new MessageResponseDto(Utils.GetLocalizedMessage($"{error.Error}", this.Localizer));
                errorResponse.Add(new ErrorValidateDto
                {
                    Name = error.Name,
                    Error = message.Message,
                });
            }

            return this.BadRequest(errorResponse);
        }

        var result = await this.userService.ResetPassword(requestDto);

        if (result)
        {
            return this.Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdatePasswordSuccess", this.localizer)));
        }
        else
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdatePasswordFailed", this.localizer)));
        }
    }
}
