using CareerTech.Model.Enums;

namespace CareerTech.Response.JobPosts;

public class JobPostPublicDetailDto
{
    required public int Id { get; set; }

    required public int RecruiterId { get; set; }

    required public string Title { get; set; }

    required public int DomainId { get; set; }

    public string? Description { get; set; }

    required public EJdPostStatus Status { get; set; }

    public DateTime? EndDate { get; set; }

    public double? MinSalary { get; set; }

    public double? MaxSalary { get; set; }

    public ECurrencySalary? CurrencySalary { get; set; }

    public EExperienceYear? ExperienceYear { get; set; }

    public EWorkingType WorkingType { get; set; }

    public int? Quantity { get; set; }

    public EDelete IsDelete { get; set; }

    public string? Requirement { get; set; }

    public string? Benefits { get; set; }

    public string? WorkLocation { get; set; }

    public string? WorkingHours { get; set; }

    public bool? IsApplied { get; set; } = false;

    public bool? IsSaved { get; set; } = false;
}
