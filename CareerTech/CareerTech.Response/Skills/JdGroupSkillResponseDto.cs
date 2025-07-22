using CareerTech.Model.Entities;

namespace CareerTech.Response.Skills;

public class JdGroupSkillResponseDto(GroupSkill groupSkill, List<JdSkillResponseDto> skills)
{
    public int Id { get; set; } = groupSkill.Id;

    public string Name { get; set; } = groupSkill.Name;

    public List<JdSkillResponseDto> Skills {  get; set; } = skills;
}
