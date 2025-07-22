using CareerTech.Model.Enums;

namespace CareerTech.Request.Authentication;

/// <summary>
/// LoginDto.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// UserName.
    /// </summary>
    required public string UserName { get; set; }

    /// <summary>
    /// Gets or sets Password.
    /// </summary>
    required public string Password { get; set; }

    /// <summary>
    /// Gets or sets Role.
    /// </summary>
    required public EUserRole Role { get; set; }

    /// <summary>
    /// Gets or sets StatyLogin.
    /// </summary>
    public string? StayLogin { get; set; }

    public string? ReturnUrl { get; set; }
}
