using CareerTech.Model.Enums;
using CareerTech.Request.Errors;

namespace CareerTech.Request.JobPosts;

public class AddJobPostApprovalDto
{
    required public int JobPostId { get; set; }

    required public int UserId { get; set; }

    public EJdPostApproval Status { get; set; }

    public EJdStatusReason? Reason { get; set; }

    public List<ErrorValidateDto> Validate()
    {
        var errors = new List<ErrorValidateDto>();

        if(this.Status < 0)
        {
            errors.Add(new ErrorValidateDto { Name = "statusJobPostApproval", Error = "errStatusCannotBeEmpty" });
        }

        return errors;
    }
}
