using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// Gets or sets Certificate.
/// </summary>
[Table("certificates")]
public class Certificate : BaseModel
{
    /// <summary>
    /// Gets or sets Name.
    /// </summary>
    [Column("name")]
    required public string Name { get; set; }

    /// <summary>
    /// Gets or sets Description.
    /// </summary>
    [Column("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets Language.
    /// </summary>
    [Column("language")]
    public string? Language { get; set; }

    /// <summary>
    /// Gets or sets AvailableLevel.
    /// </summary>
    [Column("available_level")]
    public string? AvailableLevel { get; set; }

    /// <summary>
    /// Gets or sets HtmlIcon.
    /// </summary>
    [Column("html_icon")]
    public string? HtmlIcon { get; set; }
}
