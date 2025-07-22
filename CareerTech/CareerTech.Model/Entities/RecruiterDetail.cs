using CareerTech.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// RecruiterDetail Model.
/// </summary>
[Table("recruiter_detail")]
public class RecruiterDetail : BaseModel
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
    required public string Name { get; set; }

    [Column("company_size")]
    public ECompanySize? CompanySize { get; set; }

    [Column("work_schedule")]
    public EWorkSchedule? WorkSchedule { get; set; }

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
    /// Gets or sets EstablishedDate.
    /// </summary>
    [Column("established_date")]
    public DateTime? EstablishedDate {  get; set; }

    /// <summary>
    /// Gets or sets Address.
    /// </summary>
    [Column("address")]
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets Avatar.
    /// </summary>
    [Column("avatar")]
    public string? Avatar { get; set; }

    /// <summary>
    /// Gets or sets Description.
    /// </summary>
    [Column("description")]
    public string? Description { get; set; }

    [Column("link_web")]
    public string? LinkWeb { get; set; }

    /// <summary>
    /// Gets or sets LocationMap.
    /// </summary>
    [Column("location_map")]
    public string? LocationMap { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}
