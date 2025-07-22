using CareerTech.Model.Entities;
using CareerTech.Model.Enums;

namespace CareerTech.Response.JobPosts;

public class JobPostResponseDto
{
    required public int Id { get; set; }

    required public int RecruiterId { get; set; }

    required public string NameRecruiter { get; set; }

    public string? AvatarRecruiter { get; set; }

    required public string Title { get; set; }

    public List<Province>? Province { get; set; }

    public List<Level>? Level { get; set; }

    public EExperienceYear ExperienceYear { get; set; }

    public double? MinSalary { get; set; }

    public double? MaxSalary { get; set; }

    public ECurrencySalary? CurrencySalary { get; set; }

    public DateTime UpdatedAt { get; set; }

    required public string Domain { get; set; }

    required public EWorkingType WorkingType { get; set; }

    public EJdPostStatus? Status { get; set; }

    public EJdPostApproval? StatusApproval { get; set; }
}
