using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class AdminDetailRepo : AbstractRepo<AdminDetail>, IAdminDetailRepo
{
    public AdminDetailRepo(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
