using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// JdPostCertificate.
/// </summary>
[Table("jd_posts_certificates")]
public class JdPostCertificate : BaseModel
{
    /// <summary>
    /// Gets or sets JdPostId.
    /// </summary>
    [Column("jd_post_id")]
    required public int JdPostId { get; set; }

    /// <summary>
    /// Gets or sets CertificateId.
    /// </summary>
    [Column("certificate_id")]
    required public int CertificateId { get; set; }

    /// <summary>
    /// Gets or sets JdPost.
    /// </summary>
    [ForeignKey("JdPostId")]
    public JdPost? JdPost {  get; set; }

    /// <summary>
    /// Gets or sets Certificate.
    /// </summary>
    [ForeignKey("CertificateId")]
    public Certificate? Certificate { get; set; }
}
