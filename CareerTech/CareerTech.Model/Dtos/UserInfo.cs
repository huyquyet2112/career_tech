using CareerTech.Model.Entities;
using CareerTech.Model.Enums;
using System.Security.Claims;
using System.Text.Json;

namespace CareerTech.Model.Dtos;

public class UserInfo
{
    /// <summary>
    /// Gets or sets Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets UserName.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets Name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets Avatar.
    /// </summary>
    public string? Avatar { get; set; }

    public IEnumerable<string> Permissions { get; set; }

    /// <summary>
    /// Gets or sets Role.
    /// </summary>
    public string? Role { get; set; }

    public string? VerifyStatus { get; set; }

    public UserInfo(IEnumerable<Claim> claims)
    {
        _ = int.TryParse(claims.First(claim => claim.Type == EJwtData.Id.ToString()).Value, out var id);
        this.Id = id;
        this.UserName = claims.First(claim => claim.Type == EJwtData.UserName.ToString()).Value;
        this.Name = claims.First(claim => claim.Type == EJwtData.Name.ToString()).Value;
        this.Avatar = claims.First(claim => claim.Type == EJwtData.Avatar.ToString()).Value;
        this.Role = claims.First(claim => claim.Type == EJwtData.Role.ToString()).Value;
        this.VerifyStatus = claims.First(claim => claim.Type == EJwtData.VerifyStatus.ToString()).Value;
        var permissionClaims = claims.FirstOrDefault(claim => claim.Type == EJwtData.Permissions.ToString());
        if(permissionClaims != default)
        {
            this.Permissions = JsonSerializer.Deserialize<IEnumerable<string>>(permissionClaims.Value);
        }
    }

    public UserInfo(User user, string name, string avatar)
    {
        this.Id = user.Id;
        this.UserName = user.UserName;
        this.Name = name ?? string.Empty;
        this.Role = (user.Role.ToString());
        this.VerifyStatus = (user.VerifyStatus.ToString());
        this.Avatar = avatar ?? string.Empty;
        this.Permissions = user.Permissions.Select(x => $"{x.Controller}.{x.Action}");
    }

    public UserInfo(ClaimsPrincipal claims)
    {
        _ = int.TryParse(claims.FindFirst(EJwtData.Id.ToString())?.Value, out var id);
        this.Id = id;
        this.UserName = claims.FindFirst(EJwtData.UserName.ToString())?.Value ?? string.Empty;
        this.Name = claims.FindFirst(EJwtData.Name.ToString())?.Value ?? string.Empty;
        this.Avatar = claims.FindFirst(EJwtData.Avatar.ToString())?.Value ?? string.Empty;
        this.Role = claims.FindFirst(EJwtData.Role.ToString())?.Value ?? string.Empty;
        this.VerifyStatus = claims.FindFirst(EJwtData.VerifyStatus.ToString())?.Value ?? string.Empty;
        this.Permissions = claims.FindFirst(EJwtData.Permissions.ToString()) == null ? new List<string>() : JsonSerializer.Deserialize<IEnumerable<string>>(claims.FindFirst(EJwtData.Permissions.ToString()).Value);
    }

    public bool CheckIsThisUserLoggedIn(int userId)
    {
        return this.Id == userId;
    }
}
