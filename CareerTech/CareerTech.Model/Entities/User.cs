using CareerTech.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// User Model.
/// </summary>
[Table("users")]
public class User : BaseModel
{
    /// <summary>
    /// Gets or sets UserName.
    /// </summary>
    [Column("user_name")]
    required public string UserName { get; set; }

    /// <summary>
    /// Gets or sets HashPassword.
    /// </summary>
    [Column("hash_password")]
    required public string HashPassword { get;set; }

    /// <summary>
    /// Gets or sets Status.
    /// </summary>
    [Column("status")]
    required public EUserStatus Status { get; set; }

    /// <summary>
    /// Gets or sets Role.
    /// </summary>
    [Column("role")]
    required public EUserRole Role { get; set; }

    /// <summary>
    /// Gets or sets IsVerify.
    /// </summary>
    [Column("verify_status")]
    required public EVerifyStatus VerifyStatus { get; set; }

    public AdminDetail? AdminDetail { get; set; }

    /// <summary>
    /// Not Mapper, User(1) - Permission(Many).
    /// </summary>
    [NotMapped]
    public ICollection<Permission>? Permissions { get; set; }
}
