using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;
using CareerTech.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CareerTech.Service.Services;

public class DomainService : IDomainService
{
    private readonly ILogger<DomainService> logger;
    private readonly IGroupDomainRepo groupDomainRepo;
    private readonly IDomainRepo domainRepo;

    public DomainService(
        ILogger<DomainService> logger,
        IGroupDomainRepo groupDomainRepo,
        IDomainRepo domainRepo)
    {
        this.logger = logger;
        this.groupDomainRepo = groupDomainRepo;
        this.domainRepo = domainRepo;
    }

    public async Task<IList<Domain>> GetDomains()
    {
        return await this.domainRepo.Entities.Include(us => us.GroupDomain).ToListAsync();
    }

    public async Task<IList<GroupDomain>> GetAllGroupDomain()
    {
        return await this.groupDomainRepo.GetAllAsync();
    }
}
