using CareerTech.Model.Enums;

namespace CareerTech.Response.Applicants;

public class ApplicantSavedJobPostDto
{
    required public int RecruiterId { get; set; }

    required public string RecruiterName { get; set; }

    public string? RecruiterAvatar { get; set; }

    required public int JobPostId { get; set; }

    required public string Title { get; set; }

    required public int SavedId { get; set; }

    public double? MinSalary { get; set; }

    public double? MaxSalary { get; set; }

    public ECurrencySalary? CurrencySalary { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? EndDate { get; set; }
}
