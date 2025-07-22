using CareerTech.Request.Users;
using CareerTech.Response.Users;

namespace CareerTech.Service.Interfaces;

public interface IUserService
{
    public Task<bool> ChangePassword(ChangePasswordUserDto requestDto, int userId);

    Task<SendCodeResultDto> GenerateForgotPassword(EmailForgotPasswordDto requestDto);

    Task<SendCodeResultDto> ConfirmVerificationCode(ConfirmVerificationCodeDto requestDto);

    Task<bool> ResetPassword(ResetPasswordDto requestDto);

}
