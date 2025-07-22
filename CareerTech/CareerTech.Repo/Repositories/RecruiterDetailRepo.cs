using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;


namespace CareerTech.Repo.Repositories;

public class RecruiterDetailRepo : AbstractRepo<RecruiterDetail>, IRecruiterDetailRepo
{
    public RecruiterDetailRepo(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
