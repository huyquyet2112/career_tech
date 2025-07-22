using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class JdPostRepo : AbstractRepo<JdPost>, IJdPostRepo
{
    public JdPostRepo(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
