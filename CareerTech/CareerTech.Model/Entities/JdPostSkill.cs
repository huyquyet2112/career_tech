using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// JdPostSkill.
/// </summary>
[Table("jd_posts_skills")]
public class JdPostSkill : BaseModel
{
    /// <summary>
    /// Gets or sets JdPostId.
    /// </summary>
    [Column("jd_post_id")]
    required public int JdPostId { get; set; }

    /// <summary>
    /// Gets or sets SkillId.
    /// </summary>
    [Column("skill_id")]
    required public int SkillId { get; set; }

    /// <summary>
    /// Gets or sets JdPost.
    /// </summary>
    [ForeignKey("JdPostId")]
    public JdPost? JdPost {  get; set; }

    /// <summary>
    /// Gets or sets Skill.
    /// </summary>
    [ForeignKey("SkillId")]
    public Skill? Skill { get; set; }
}
