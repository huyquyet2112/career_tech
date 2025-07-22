namespace CareerTech.Request.Applicants;

public class UpdateApplicantProvinceDto
{
    required public int UserId { get; set; }

    required public List<int> Provinces { get; set; }
}
