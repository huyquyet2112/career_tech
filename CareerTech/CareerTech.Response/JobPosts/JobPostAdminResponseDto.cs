using CareerTech.Model.Enums;

namespace CareerTech.Response.JobPosts;

public class JobPostAdminResponseDto
{
    public int JobPostId { get; set; }

    public string? Title { get; set; }

    public EJdPostStatus? Status { get; set; }

    public EJdPostApproval? StatusApproval {  get; set; }

    public int RecruiterId { get; set; }

    public string? NameRecruiter { get; set; }

    public int UserId { get; set; }
}
