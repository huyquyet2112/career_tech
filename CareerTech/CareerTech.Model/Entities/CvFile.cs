using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// CvFile Model.
/// </summary>
[Table("cv_files")]
public class CvFile : BaseModel
{
    /// <summary>
    /// Gets or sets UserId.
    /// </summary>
    [Column("user_id")]
    public int? UserId { get; set; }

    /// <summary>
    /// Gets or sets Name.
    /// </summary>
    [Column("name")]
    required public string Name { get; set; }

    /// <summary>
    /// Gets or sets UrlFile.
    /// </summary>
    [Column("url_file")]
    required public string UrlFile { get; set; }
}
