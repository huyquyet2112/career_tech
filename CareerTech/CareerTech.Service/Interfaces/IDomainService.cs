using CareerTech.Model.Entities;

namespace CareerTech.Service.Interfaces;

public interface IDomainService
{
    Task<IList<Domain>> GetDomains();

    Task<IList<GroupDomain>> GetAllGroupDomain();
}
