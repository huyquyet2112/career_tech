namespace CareerTech.Request.Applicants;

public class UpdateApplicantSkillDto
{
    required public int UserId { get; set; }

    required public List<int> Skills { get; set; } = [];
}
