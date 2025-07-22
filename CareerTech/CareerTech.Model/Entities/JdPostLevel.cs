using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// JdPostLevel.
/// </summary>
[Table("jd_posts_levels")]
public class JdPostLevel : BaseModel
{
    /// <summary>
    /// JdPostId.
    /// </summary>
    [Column("jd_post_id")]
    required public int JdPostId { get; set; }

    /// <summary>
    /// LevelId.
    /// </summary>
    [Column("level_id")]
    required public int LevelId { get; set; }

    /// <summary>
    /// JdPost.
    /// </summary>
    [ForeignKey("JdPostId")]
    public JdPost? JdPost { get; set; }

    /// <summary>
    /// Level.
    /// </summary>
    [ForeignKey("LevelId")]
    public Level? Level { get; set; }
}
