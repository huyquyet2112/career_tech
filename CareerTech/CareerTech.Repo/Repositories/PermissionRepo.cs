using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;


namespace CareerTech.Repo.Repositories;

public class PermissionRepo : AbstractRepo<Permission>, IPermissionRepo
{
    public PermissionRepo(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
