using CareerTech.Model.Enums;

namespace CareerTech.Response.Applicants;

public class ApplicantAppliedJobPostDto
{
    required public int RecruiterId { get; set; }

    required public string RecruiterName { get; set; }

    public string? RecruiterAvatar {  get; set; }

    required public int JobPostId { get; set; }

    required public string Title { get; set; }

    required public DateTime AppliedAt { get; set; }

    public string? UrlFile  { get; set; }

    public double? MinSalary { get; set; }

    public double? MaxSalary { get; set; }

    public ECurrencySalary? CurrencySalary { get; set; }

    public EApplyJobStatus Status { get; set; }

    public DateTime? ViewedAt { get; set; }

    public EFitApplyJob? FitStatus {  get; set; }

    public DateTime? ReviewedAt { get; set; }

}
