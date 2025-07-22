using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// Gets or sets RecruiterDomain.
/// </summary>
[Table("recruiter_domain")]
public class RecruiterDomain : BaseModel
{
    /// <summary>
    /// Gets or sets UserId.
    /// </summary>
    [Column("user_id")]
    required public int UserId { get; set; }

    /// <summary>
    /// Gets or sets DomainId.
    /// </summary>
    [Column("group_domain_id")]
    required public int GroupDomainId { get; set; }

    /// <summary>
    /// Gets or sets User.
    /// </summary>
    [ForeignKey("UserId")]
    public User? User { get; set; }

    /// <summary>
    /// Gets or sets Domain.
    /// </summary>
    [ForeignKey("GroupDomainId")]
    public GroupDomain? GroupDomain { get; set; }
}
