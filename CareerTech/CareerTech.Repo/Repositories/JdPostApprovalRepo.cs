using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class JdPostApprovalRepo : AbstractRepo<JdPostApproval>, IJdPostApprovalRepo
{
    public JdPostApprovalRepo(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
