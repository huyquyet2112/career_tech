using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// Domain.
/// </summary>
[Table("domains")]
public class Domain : BaseModel
{
    /// <summary>
    /// Gets or sets Name.
    /// </summary>
    required public string Name { get; set; }

    /// <summary>
    /// Gets or sets GroupDomainId.
    /// </summary>
    [Column("group_domain_id")]
    required public int GroupDomainId { get; set; }

    [ForeignKey("GroupDomainId")]
    public GroupDomain? GroupDomain { get; set; }
}
