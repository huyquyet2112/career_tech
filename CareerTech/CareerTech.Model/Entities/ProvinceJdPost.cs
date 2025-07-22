using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// ProvinceJdPost.
/// </summary>
[Table("provinces_jd_posts")]
public class ProvinceJdPost : BaseModel
{
    /// <summary>
    /// JdPostId.
    /// </summary>
    [Column("jd_post_id")]
    required public int JdPostId { get; set; }

    /// <summary>
    /// ProvinceId.
    /// </summary>
    [Column("province_id")]
    required public int ProvinceId { get; set; }

    /// <summary>
    /// JdPostId.
    /// </summary>
    [ForeignKey("JdPostId")]
    public JdPost? JdPost { get; set; }

    /// <summary>
    /// Gets or sets Provinces.
    /// </summary>
    [ForeignKey("ProvinceId")]
    public Province? Provinces { get; set; }
}
