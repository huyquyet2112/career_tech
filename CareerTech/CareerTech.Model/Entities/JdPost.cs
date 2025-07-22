using CareerTech.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerTech.Model.Entities;

/// <summary>
/// JdPost Model.
/// </summary>
[Table("jd_posts")]
public class JdPost : BaseModel
{
    /// <summary>
    /// Gets or sets UserId.
    /// </summary>
    [Column("user_id")]
    required public int UserId { get; set; }

    /// <summary>
    /// Gets or sets Title.
    /// </summary>
    [Column("title")]
    required public string Title { get; set; }

    /// <summary>
    /// Gets or sets JobTitleId.
    /// </summary>
    [Column("domain_id")]
    required public int DomainId { get; set; }

    /// <summary>
    /// Gets or sets Description.
    /// </summary>
    [Column("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets Status.
    /// </summary>
    [Column("status")]
    required public EJdPostStatus Status { get; set; }

    /// <summary>
    /// Gets or sets EndDate.
    /// </summary>
    [Column("end_date")]
    public DateTime? EndDate {  get; set; }

    /// <summary>
    /// Gets or sets MinSalary.
    /// </summary>
    [Column("min_salary")]
    public double? MinSalary { get; set; }

    /// <summary>
    /// Gets or sets MaxSalary.
    /// </summary>
    [Column("max_salary")]
    public double? MaxSalary { get; set; }

    /// <summary>
    /// Gets or sets CurrencySalary.
    /// </summary>
    [Column("currency_salary")]
    public ECurrencySalary? CurrencySalary { get; set; }

    /// <summary>
    /// Gets or sets WorkingType.
    /// </summary>
    [Column("working_type")]
    public EWorkingType WorkingType {  get; set; }

    /// <summary>
    /// Gets or sets Quantity.
    /// </summary>
    [Column("quantity")]
    public int? Quantity { get; set; }

    /// <summary>
    /// Gets or sets IsDelete.
    /// </summary>
    [Column("is_delete")]
    public EDelete IsDelete { get; set; }

    /// <summary>
    /// Gets or sets Requirement.
    /// </summary>
    [Column("requirement")]
    public string? Requirement { get; set; }

    /// <summary>
    /// Gets or sets Benefits.
    /// </summary>
    [Column("benefits")]
    public string? Benefits { get; set; }

    /// <summary>
    /// Gets or sets Work
    /// </summary>
    [Column("work_location")]
    public string? WorkLocation {  get; set; }

    /// <summary>
    /// Gets or sets WorkingHours.
    /// </summary>
    [Column("working_hours")]
    public string? WorkingHours { get; set; }

    /// <summary>
    /// Gets or sets ExperienceLevel.
    /// </summary>
    [Column("experience_year")]
    required public EExperienceYear ExperienceYear { get; set; }

    /// <summary>
    /// Gets or sets User.
    /// </summary>
    [ForeignKey("UserId")]
    public User? User { get; set; }

    /// <summary>
    /// Gets or sets Domain.
    /// </summary>
    [ForeignKey("DomainId")]
    public Domain? Domain { get; set; }

    /// <summary>
    /// Gets or sets JdPostLevel.
    /// </summary>
    public ICollection<JdPostLevel> JdPostLevel { get; set; } = new List<JdPostLevel>();

    /// <summary>
    /// Gets or sets ProvinceJdPost.
    /// </summary>
    public ICollection<ProvinceJdPost> ProvinceJdPosts { get; set; } = new List<ProvinceJdPost>();

    public ICollection<JdPostApproval> JdPostApprovals { get; set; } = new List<JdPostApproval>();
}
