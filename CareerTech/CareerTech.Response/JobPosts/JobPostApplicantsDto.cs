using CareerTech.Model.Enums;

namespace CareerTech.Response.JobPosts;

public class JobPostApplicantsDto
{
    required public int ApplyId { get; set; }

    required public int UserId { get; set; }

    required public string NameApplicant { get; set; }

    required public int FileId { get; set; }

    required public string FileName { get; set; }

    required public string UrlFile { get; set; }

    required public EApplyJobStatus Status { get; set; }

    public EFitApplyJob? FitApplyJob { get; set; }

    required public DateTime CreatedAt { get; set; }
}
