using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class GroupSkillRepo : AbstractRepo<GroupSkill>, IGroupSkillRepo
{
    public GroupSkillRepo(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
