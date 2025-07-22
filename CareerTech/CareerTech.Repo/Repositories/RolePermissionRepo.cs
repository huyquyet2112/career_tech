using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;


namespace CareerTech.Repo.Repositories;

public class RolePermissionRepo : AbstractRepo<RolePermission>, IRolePermissionRepo
{
    public RolePermissionRepo(DatabaseContext dbContext): base(dbContext)
    {

    }
}
