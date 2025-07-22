using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// Skill.
/// </summary>
[Table("skills")]
public class Skill : BaseModel
{
    /// <summary>
    /// Gets or sets Name.
    /// </summary>
    [Column("name")]
    required public string Name { get; set; }

    /// <summary>
    /// Gets or sets GroupSkillId.
    /// </summary>
    [Column("group_skill_id")]
    required public int GroupSkillId { get; set; }

    /// <summary>
    /// Gets or sets Description.
    /// </summary>
    [Column("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets HtmlIcon.
    /// </summary>
    [Column("html_icon")]
    public string? HtmlIcon { get; set; }

    /// <summary>
    /// Gets or sets GroupSkill.
    /// </summary>
    [ForeignKey("GroupSkillId")]
    public GroupSkill? GroupSkill { get; set; }
}
