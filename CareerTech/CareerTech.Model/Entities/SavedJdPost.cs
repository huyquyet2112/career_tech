using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

[Table("saved_jd_posts")]
public class SavedJdPost : BaseModel
{
    /// <summary>
    /// Gets or sets UserId.
    /// </summary>
    [Column("user_id")]
    required public int UserId { get; set; }

    /// <summary>
    /// Gets or sets JdPostId.
    /// </summary>
    [Column("jd_post_id")]
    required public int JdPostId { get; set; }

    /// <summary>
    /// Gets or sets User.
    /// </summary>
    [ForeignKey("UserId")]
    public User? User { get; set; }

    /// <summary>
    /// Gets or sets JdPost.
    /// </summary>
    [ForeignKey("JdPostId")]
    public JdPost? JdPost { get; set; }
}
