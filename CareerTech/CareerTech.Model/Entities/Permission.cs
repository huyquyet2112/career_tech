using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// Permission Model.
/// </summary>
[Table("permissions")]
public class Permission : BaseModel
{
    /// <summary>
    /// Gets or sets Controller.
    /// </summary>
    [Column("controller")]
    required public string Controller { get; set; }

    /// <summary>
    /// Gets or sets Action.
    /// </summary>
    [Column("action")]
    required public string Action { get; set; }

    /// <summary>
    /// Gets or sets Method.
    /// </summary>
    [Column("method")]
    required public string Method { get; set; }

    /// <summary>
    /// Gets or sets Url.
    /// </summary>
    [Column("url")]
    required public string Url { get; set; }

    [Column("name")]
    required public string Name { get; set; }
}
