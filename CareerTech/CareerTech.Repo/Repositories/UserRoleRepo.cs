using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class UserRoleRepo : AbstractRepo<UserRole>, IUserRoleRepo
{
    public UserRoleRepo(DatabaseContext dbContext): base(dbContext)
    {

    }
}
