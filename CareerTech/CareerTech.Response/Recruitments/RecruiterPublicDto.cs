using CareerTech.Model.Entities;
using CareerTech.Model.Enums;

namespace CareerTech.Response.Recruitments;

public class RecruiterPublicDto
{
    required public int RecruiterId { get; set; }  

    required public string RecruiterName { get; set; }

    public string? RecuiterAvatar { get; set; }

    public EWorkSchedule? WorkSchedule { get; set; }

    public ECompanySize? CompanySize { get; set; }

    public int JobPostPuclic { get; set; }

    required public int UserId { get; set; }

    public EUserStatus? Status { get; set; }

    public string? Description { get; set; }

    required public EVerifyStatus VerifyStatus { get; set; }
}
