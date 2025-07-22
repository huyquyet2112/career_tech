using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

[Table("applicants_provinces")]
public class ApplicantProvince : BaseModel
{
    [Column("user_id")]
    required public int UserId { get; set; }

    [Column("province_id")]
    required public int ProvinceId { get; set; }

    [ForeignKey("ProvinceId")]
    public Province? Province { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}
