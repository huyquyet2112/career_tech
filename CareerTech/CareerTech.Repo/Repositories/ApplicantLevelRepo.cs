using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class ApplicantLevelRepo : AbstractRepo<ApplicantLevel>, IApplicantLevelRepo
{
    public ApplicantLevelRepo(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
