using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class JdPostLevelRepo : AbstractRepo<JdPostLevel>, IJdPostLevelRepo
{
    public JdPostLevelRepo(DatabaseContext dbContext) : base(dbContext) { }
}
