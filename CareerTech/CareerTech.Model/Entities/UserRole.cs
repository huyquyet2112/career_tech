using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// UserRole Model.
/// </summary>
[Table("users_roles")]
public class UserRole : BaseModel
{
    /// <summary>
    /// Gets or sets UserId.
    /// </summary>
    [Column("user_id")]
    required public int UserId { get; set; }

    /// <summary>
    /// Gets or sets RoleId.
    /// </summary>
    [Column("role_id")]
    required public int RoleId { get; set; }
}
