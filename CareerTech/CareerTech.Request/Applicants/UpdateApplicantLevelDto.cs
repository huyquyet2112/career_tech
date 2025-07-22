namespace CareerTech.Request.Applicants;

public class UpdateApplicantLevelDto
{
    required public int UserId { get; set; }

    required public List<int> Levels { get; set; }
}
