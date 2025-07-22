using CareerTech.Model.Context;
using CareerTech.Repo.Interfaces;
using CareerTech.Model.Entities;



namespace CareerTech.Repo.Repositories;

public class RoleRepo : AbstractRepo<Role>, IRoleRepo
{
    public RoleRepo(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
