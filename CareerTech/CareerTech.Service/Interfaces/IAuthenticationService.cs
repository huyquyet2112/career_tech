using CareerTech.Model.Dtos;
using CareerTech.Request.Authentication;

namespace CareerTech.Service.Interfaces;

public interface IAuthenticationService
{
    /// <summary>
    /// ValidateToken.
    /// </summary>
    /// <param name="token">token.</param>
    /// <returns>UserInfo.</returns>
    public UserInfo? ValidateToken(string? token);

    public string GenerateJWTAuthentication(UserInfo user, DateTime expiredTime);

    public Task<string> Login(LoginDto loginDto);

    Task<bool> Register(RegisterDto registerDto);
}
