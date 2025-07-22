using CareerTech.Model.Enums;

namespace CareerTech.Request.Authentication;

public class RegisterDto
{
    required public EUserRole Role { get; set; }

    required public string Name { get; set; }

    required public string UserName { get; set; }

    required public string Password { get; set; }

    required public string ConfirmPassword { get; set; }


}
