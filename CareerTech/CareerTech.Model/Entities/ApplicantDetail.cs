using CareerTech.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// CandidateDetail Model.
/// </summary>
[Table("applicant_detail")]
public class ApplicantDetail : BaseModel
{
    /// <summary>
    /// Gets or sets UserId.
    /// </summary>
    [Column("user_id")]
    required public int UserId { get; set; }

    /// <summary>
    /// Gets or sets Name.
    /// </summary>
    [Column("name")]
    required public string Name { get;set; }

    /// <summary>
    /// Gets or sets Gender.
    /// </summary>
    [Column("gender")]
    public EGender? Gender { get; set; }

    /// <summary>
    /// Gets or sets Birthday.
    /// </summary>
    [Column("birthday")]
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// Gets or sets Email.
    /// </summary>
    [Column("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets PhoneNumber.
    /// </summary>
    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets Address.
    /// </summary>
    [Column("address")]
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets Description.
    /// </summary>
    [Column("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets Avatar.
    /// </summary>
    [Column("avatar")]
    public string? Avatar { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}
