using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;


namespace CareerTech.Repo.Repositories;

public class GroupDomainRepo : AbstractRepo<GroupDomain>, IGroupDomainRepo
{
    public GroupDomainRepo(DatabaseContext dbContext): base(dbContext)
    {

    }
}
