using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// AdminDetail Model.
/// </summary>
[Table("admin_detail")]
public class AdminDetail : BaseModel
{
    /// <summary>
    /// Gets or sets Name.
    /// </summary>
    [Column("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets AdminCode.
    /// </summary>
    [Column("admin_code")]
    required public string AdminCode { get; set; }

    /// <summary>
    /// Gets or sets Avatar.
    /// </summary>
    [Column("avatar")]
    public string? Avatar { get; set; }

    /// <summary>
    /// Gets or sets UserId.
    /// </summary>
    [Column("user_id")]
    required public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}
