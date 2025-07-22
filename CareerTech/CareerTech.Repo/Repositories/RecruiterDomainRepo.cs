using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class RecruiterDomainRepo : AbstractRepo<RecruiterDomain>, IRecruiterDomainRepo
{
    public RecruiterDomainRepo(DatabaseContext dbContext): base(dbContext)
    {

    }
}
