namespace CareerTech.Request.Applicants;

public class QueryApplicantPublicDto
{
    public string? Name { get; set; }

    public List<int>? Levels { get; set; }

    public List<int>? Provinces { get; set; }

    public List<int>? Skills { get; set; }

    public bool IsSelectedLevel(int leveId)
    {
        if (this.Levels == null)
        {
            return false;
        }

        return this.Levels.Contains(leveId);
    }

    public bool IsSelectedProvince(int provinceId)
    {
        if (this.Provinces == null)
        {
            return false;
        }

        return this.Provinces.Contains(provinceId);
    }

    public bool IsSelectedSkill(int skillId)
    {
        if (this.Skills == null)
        {
            return false;
        }

        return this.Skills.Contains(skillId);
    }
}