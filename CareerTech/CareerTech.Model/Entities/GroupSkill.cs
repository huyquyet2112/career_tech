using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// GroupSkill.
/// </summary>
[Table("group_skills")]
public class GroupSkill : BaseModel
{
    /// <summary>
    /// Gets or sets Name.
    /// </summary>
    [Column("name")]
    required public string Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }
}
