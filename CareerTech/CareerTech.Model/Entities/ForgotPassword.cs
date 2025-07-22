using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

[Table("forgot_password")]
public class ForgotPassword : BaseModel
{
    [Column("email")]
    required public string Email { get; set; }

    [Column("code")]
    required public string Code { get; set; }

    [Column("expired_at")]
    required public DateTime ExpiredAt { get; set; }
}
