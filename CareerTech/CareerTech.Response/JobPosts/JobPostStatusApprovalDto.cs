using CareerTech.Model.Enums;

namespace CareerTech.Response.JobPosts;

public class JobPostStatusApprovalDto
{
    public int Id { get; set; }

    public EJdPostApproval? Status { get; set; }

    public EJdStatusReason? Reason { get; set; }

    public string? UserApproval {  get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
