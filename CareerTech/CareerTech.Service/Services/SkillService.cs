using CareerTech.Service.Interfaces;
using CareerTech.Model.Entities;
using Microsoft.Extensions.Logging;
using CareerTech.Repo.Interfaces;
using CareerTech.Model.Context;
using Microsoft.EntityFrameworkCore;
using CareerTech.Response.Skills;
using CareerTech.Request.Applicants;

namespace CareerTech.Service.Services;

public class SkillService : ISkillService
{
    private readonly IGroupSkillRepo groupSkillRepo;
    private readonly ISkillRepo skillRepo;
    private readonly IJdPostSkillRepo jdPostSkillRepo;
    private readonly IApplicantSkillRepo applicantSkillRepo;
    private readonly IUserRepo userRepo;
    private readonly DatabaseContext databaseContext;

    public SkillService(
        IGroupSkillRepo groupSkillRepo,
        ISkillRepo skillRepo,
        IJdPostSkillRepo jdPostSkillRepo,
        IApplicantSkillRepo applicantSkillRepo,
        IUserRepo userRepo,
        DatabaseContext databaseContext)
    {
        this.groupSkillRepo = groupSkillRepo;
        this.skillRepo = skillRepo;
        this.jdPostSkillRepo = jdPostSkillRepo;
        this.applicantSkillRepo = applicantSkillRepo;
        this.userRepo = userRepo;
        this.databaseContext = databaseContext;
    }

    public async Task<IList<Skill>> GetSkills()
    {
        return await this.skillRepo.Entities.Include(us => us.GroupSkill).ToListAsync();
    }

    public async Task<IList<JdGroupSkillResponseDto>> GetJdGroupSkillS(int? jdPostId)
    {
        var skills = await this.GetSkills();
        var jdGroupSkillDtos = new List<JdGroupSkillResponseDto>();
        var groupSkills = skills.GroupBy(s => s.GroupSkillId);

        var jdSkills = jdPostId != null
            ? await this.jdPostSkillRepo.FindManyAsync(us => us.JdPostId == jdPostId)
            : new List<JdPostSkill>();

        foreach (var groupSkill in groupSkills)
        {
            var jdSkillDtos = new List<JdSkillResponseDto>();
            GroupSkill? group = null;

            foreach (var skill in groupSkill)
            {
                if (skill != null)
                {
                    group = skill.GroupSkill;
                    bool selected = jdSkills.Any(us => us.SkillId == skill.Id);
                    jdSkillDtos.Add(new JdSkillResponseDto(skill, selected));
                }
            }

            if (group != null)
            {
                jdGroupSkillDtos.Add(new JdGroupSkillResponseDto(group, jdSkillDtos));
            }
        }

        return jdGroupSkillDtos;
    }

    public async Task<IList<GroupSkillResponseDto>> GetApplicantGroupSkills(int? userId)
    {
        var skills = await this.GetSkills();
        var applicantGroupSkillDtos = new List<GroupSkillResponseDto>();
        var groupSkills = skills.GroupBy(s => s.GroupSkillId);

        var applicantSkills = userId != null
            ? await this.applicantSkillRepo.FindManyAsync(us => us.UserId == userId)
            : new List<ApplicantSkill>();

        foreach (var groupSkill in groupSkills)
        {
            var applicantSkillDtos = new List<SkillResponseDto>();
            GroupSkill? group = null;

            foreach (var skill in groupSkill)
            {
                if (skill != null)
                {
                    group = skill.GroupSkill;
                    bool selected = applicantSkills.Any(us => us.SkillId == skill.Id);
                    applicantSkillDtos.Add(new SkillResponseDto(skill, selected));
                }
            }

            if (group != null)
            {
                applicantGroupSkillDtos.Add(new GroupSkillResponseDto(group, applicantSkillDtos));
            }
        }

        return applicantGroupSkillDtos;
    }

    public async Task<bool> UpdateApplicantSkill(UpdateApplicantSkillDto requestDto)
    {
        var user = await this.userRepo.FindOneAsync(us => us.Id == requestDto.UserId);

        if (user == default)
        {
            throw new Exception("errUserNotFound");
        }

        var transaction = this.databaseContext.Database.BeginTransaction();

        try
        {
            if (requestDto.Skills.Count == 0)
            {
                var removeSkills = await this.applicantSkillRepo.FindManyAsync(us => us.UserId == requestDto.UserId);
                this.applicantSkillRepo.Remove(removeSkills);
            }
            else
            {
                var olderSkills = await this.applicantSkillRepo.FindManyAsync(us => us.UserId == requestDto.UserId);
                var requestSkills = await this.skillRepo.FindManyAsync(us => requestDto.Skills.Contains(us.Id));
                var removeSkills = olderSkills.ExceptBy(requestSkills.Select(us => us.Id), uq => uq.SkillId).ToList();
                var addSkills = requestSkills.ExceptBy(olderSkills.Select(us => us.SkillId), uq => uq.Id).ToList();

                if (removeSkills.Count != 0)
                {
                    this.applicantSkillRepo.Remove(removeSkills);
                }

                if (addSkills.Count != 0)
                {
                    var applicantSkills = addSkills.Select(skill => new ApplicantSkill
                    {
                        SkillId = skill.Id,
                        UserId = requestDto.UserId,
                    });

                    await this.applicantSkillRepo.SaveManyAsync(applicantSkills);
                }
            }

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }
}
