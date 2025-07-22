using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class SkillRepo : AbstractRepo<Skill>, ISkillRepo
{
    public SkillRepo(DatabaseContext dbContext) : base(dbContext)
    {
        
    }
}
