namespace CareerTech.Request.Users;

public class ConfirmVerificationCodeDto
{
    required public string Email { get; set; }

    required public string Code { get; set; }
}
