using CareerTech.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// ApplyJob Model.
/// </summary>
[Table("apply_job")]
public class ApplyJob : BaseModel
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
    /// Gets or sets CvFileId.
    /// </summary>
    [Column("cv_file_id")]
    public int CvFileId { get; set; }

    /// <summary>
    /// Gets or sets Status.
    /// </summary>
    [Column("status")]
    public EApplyJobStatus Status { get; set; }

    [Column("fit_status")]
    public EFitApplyJob? FitStatus { get; set; }

    /// <summary>
    /// Gets or sets Description.
    /// </summary>
    [Column("description")]
    public string? Description {  get; set; }

    [Column("viewed_at")]
    public DateTime? ViewedAt { get; set; }

    [Column("reviewed_at")]
    public DateTime? ReviewAt { get; set; }

    /// <summary>
    /// Gets or sets User.
    /// </summary>
    [ForeignKey("UserId")]
    public User? User { get; set; }

    /// <summary>
    /// Gets or sets CvFile.
    /// </summary>
    [ForeignKey("CvFileId")]
    public CvFile? CvFile { get; set; }

    /// <summary>
    /// Gets or sets JdPost.
    /// </summary>
    [ForeignKey("JdPostId")]
    public JdPost? JdPost { get; set; }
}
