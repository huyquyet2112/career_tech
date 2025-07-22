using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// RolePermission Model.
/// </summary>
[Table("roles_permissions")]
public class RolePermission : BaseModel
{
    /// <summary>
    /// Gets or sets RoleId.
    /// </summary>
    [Column("role_id")]
    required public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets PermissionId.
    /// </summary>
    [Column("permission_id")]
    required public int PermissionId { get; set; }
}
