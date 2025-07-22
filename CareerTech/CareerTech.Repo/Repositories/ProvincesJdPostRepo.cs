using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class ProvincesJdPostRepo : AbstractRepo<ProvinceJdPost>, IProvincesJdPostRepo
{
    public ProvincesJdPostRepo(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
