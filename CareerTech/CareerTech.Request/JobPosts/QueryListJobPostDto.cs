using CareerTech.Model.Enums;

namespace CareerTech.Request.JobPosts;

public class QueryListJobPostDto
{
    required public int Page { get; set; } = 1;

    required public int PageSize { get; set; } = 5;

    public List<int>? Levels { get; set; }

    public List<int>? Provinces { get; set; }

    public List<EWorkingType>? WorkingType { get; set; }

    public List<int>? Skills { get; set; }

    public string? Title { get; set; }

    public bool IsSelectedLevel(int levelId)
    {
        if(this.Levels == null)
        {
            return false;
        }

        return this.Levels.Contains(levelId);
    }

    public bool IsSelectedProvince(int provinceId)
    {
        if(this.Provinces == null)
        {
            return false;
        }

        return this.Provinces.Contains(provinceId);
    }

    public bool IsSelectedWorkingType(EWorkingType workingType)
    {
        if (this.WorkingType == null)
        {
            return false;
        }

        return this.WorkingType.Contains(workingType);
    }

    public bool IsSelectedSkill(int skillId)
    {
        if(this.Skills == null)
        {
            return false;
        }

        return this.Skills.Contains(skillId);
    }

    public int GetSkip()
    {
        if (this.Page <= 0)
        {
            return 0;
        }

        return (this.Page - 1) * this.PageSize;
    }
}
