using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class ApplyJobRepo : AbstractRepo<ApplyJob>, IApplyJobRepo
{
    public ApplyJobRepo(DatabaseContext dbContext): base(dbContext)
    {

    }
}
