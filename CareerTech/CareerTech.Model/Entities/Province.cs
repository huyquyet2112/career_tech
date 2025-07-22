using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// Provinces.
/// </summary>
[Table("provinces")]
public class Province : BaseModel
{
    /// <summary>
    /// Gets or sets Name.
    /// </summary>
    [Column("name")]
    required public string Name { get; set; }
}
