using CareerTech.Model.Entities;

namespace CareerTech.Response.Skills;

public class GroupSkillResponseDto(GroupSkill groupSkill, List<SkillResponseDto> skills)
{
    public int Id { get; set; } = groupSkill.Id;

    public string Name { get; set; } = groupSkill.Name;

    public List<SkillResponseDto> Skills { get; set; } = skills;
}
