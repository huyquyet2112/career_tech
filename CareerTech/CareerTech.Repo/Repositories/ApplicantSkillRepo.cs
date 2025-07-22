using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class ApplicantSkillRepo : AbstractRepo<ApplicantSkill>, IApplicantSkillRepo
{
    public ApplicantSkillRepo(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
