using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

[Table("applicants_certificates")]
public class ApplicantCertificate : BaseModel
{
    /// <summary>
    /// Gets or sets UserId.
    /// </summary>
    [Column("user_id")]
    required public int UserId { get; set; }

    /// <summary>
    /// Gets or sets CertificateId.
    /// </summary>
    [Column("certificate_id")]
    required public int CertificateId { get; set; }

    /// <summary>
    /// Certificate.
    /// </summary>
    [ForeignKey("CertificateId")]
    public Certificate? Certificate { get; set; }

    /// <summary>
    /// Gets or sets User.
    /// </summary>
    [ForeignKey("UserId")]
    public User? User { get; set; }
}
