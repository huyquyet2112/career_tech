using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class JdPostSkillRepo : AbstractRepo<JdPostSkill>, IJdPostSkillRepo
{
    public JdPostSkillRepo(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
