using CareerTech.Common.Utils;
using CareerTech.Request.Errors;

namespace CareerTech.Request.Users;

/// <summary>
/// ChangePasswordUserDto.
/// </summary>
public class ChangePasswordUserDto
{
    /// <summary>
    /// Gets or sets NewPassword.
    /// </summary>
    required public string NewPassword { get; set; }

    /// <summary>
    /// Gets or sets ConfirmPassword.
    /// </summary>
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
