using CareerTech.Model.Entities;

namespace CareerTech.Response.Skills;

/// <summary>
/// JdSkillResponseDto.
/// </summary>
/// <param name="skill">represent for Skill.</param>
/// <param name="isSelected">isSelected.</param>
public class JdSkillResponseDto(Skill skill, bool isSelected = false) 
{
    public int Id { get; set; } = skill.Id;

    public string Name { get; set; } = skill.Name;

    public string? GroupName { get; set; } = skill.GroupSkill?.Name ?? string.Empty;

    public bool IsSelected { get; set; } = isSelected;
}
