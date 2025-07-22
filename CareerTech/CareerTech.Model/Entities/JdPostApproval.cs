using System.ComponentModel.DataAnnotations.Schema;
using CareerTech.Model.Enums;

namespace CareerTech.Model.Entities;

/// <summary>
/// JdPostApproval.
/// </summary>
[Table("jd_posts_approvals")]
public class JdPostApproval : BaseModel
{
    /// <summary>
    /// Gets or sets JdPostId.
    /// </summary>
    [Column("jd_post_id")]
    required public int JdPostId { get; set; }

    /// <summary>
    /// Gets or sets UserId.
    /// </summary>
    [Column("user_id")]
    public int? UserId { get; set; }

    /// <summary>
    /// Gets or sets Status.
    /// </summary>
    [Column("status")]
    public EJdPostApproval Status { get; set; }

    /// <summary>
    /// Gets or sets Reason.
    /// </summary>
    [Column("reason")]
    public EJdStatusReason? Reason { get; set; }

    /// <summary>
    /// User.
    /// </summary>
    [ForeignKey("UserId")]
    public User? User { get; set; }

    /// <summary>
    /// JdPost.
    /// </summary>
    [ForeignKey("JdPostId")]
    public JdPost? JdPost {  get; set; }
}
