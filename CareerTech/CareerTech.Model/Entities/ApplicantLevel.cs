
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

[Table("applicants_levels")]
public class ApplicantLevel : BaseModel
{
    [Column("user_id")]
    required public int UserId { get; set; }

    [Column("level_id")]
    required public int LevelId { get; set; }

    [ForeignKey("LevelId")]
    public Level? Level { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}
