using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;


namespace CareerTech.Repo.Repositories;

public class ApplicantDetailRepo : AbstractRepo<ApplicantDetail>, IApplicantDetailRepo
{
    public ApplicantDetailRepo(DatabaseContext dbContext): base(dbContext)
    {

    }
}
