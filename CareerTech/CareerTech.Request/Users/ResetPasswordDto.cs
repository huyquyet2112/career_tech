using CareerTech.Common.Utils;
using CareerTech.Request.Errors;

namespace CareerTech.Request.Users;

public class ResetPasswordDto
{
    required public string Email { get; set; }

    required public string NewPassword { get; set; }

    required public string ConfirmPassword { get; set; }

    public List<ErrorValidateDto> Validate()
    {
        var errorValidates = new List<ErrorValidateDto>();

        if (this.NewPassword == null)
        {
            errorValidates.Add(new ErrorValidateDto { Name = "newPassword", Error = "errPasswordCannotBeEmpty" });
        }

        if (this.ConfirmPassword == null)
        {
            errorValidates.Add(new ErrorValidateDto { Name = "confirmPassword", Error = "errConfirmPasswordCannotBeEmpty" });
        }

        if (this.NewPassword != null && this.ConfirmPassword != null && !this.ConfirmPassword.Equals(NewPassword))
        {
            errorValidates.Add(new ErrorValidateDto { Name = "differentPassword", Error = "errPasswordNotEqual" });
        }

        if (this.NewPassword != null && !Helper.ICorrectPasswordFormat(NewPassword))
        {
            errorValidates.Add(new ErrorValidateDto { Name = "passwordStrong", Error = "errPasswordDoNotStrong" });
        }

        return errorValidates;
    }
}
