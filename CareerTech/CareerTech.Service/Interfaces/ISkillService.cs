using CareerTech.Model.Entities;
using CareerTech.Request.Applicants;
using CareerTech.Response.Skills;

namespace CareerTech.Service.Interfaces;

public interface ISkillService
{
    Task<IList<Skill>> GetSkills();

    Task<IList<JdGroupSkillResponseDto>> GetJdGroupSkillS(int? jdPostId);

    Task<IList<GroupSkillResponseDto>> GetApplicantGroupSkills(int? userId);

    Task<bool> UpdateApplicantSkill(UpdateApplicantSkillDto requestDto);
}
