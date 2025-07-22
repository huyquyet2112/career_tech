using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class ApplicantProvinceRepo : AbstractRepo<ApplicantProvince>, IApplicantProvinceRepo
{
    public ApplicantProvinceRepo(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
