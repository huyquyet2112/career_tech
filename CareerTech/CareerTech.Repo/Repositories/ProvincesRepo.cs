using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class ProvincesRepo : AbstractRepo<Province>, IProvincesRepo
{
    public ProvincesRepo(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
