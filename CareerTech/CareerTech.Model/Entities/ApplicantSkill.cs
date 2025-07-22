using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// ApplicantSkill.
/// </summary>
[Table("applicants_skills")]
public class ApplicantSkill : BaseModel
{
    /// <summary>
    /// Gets or sets UserId.
    /// </summary>
    [Column("user_id")]
    required public int UserId { get; set; }

    [Column("skill_id")]
    required public int SkillId { get; set; }

    [ForeignKey("SkillId")]
    public Skill? Skill { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}
