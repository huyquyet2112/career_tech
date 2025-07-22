using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;
using CareerTech.Request.Applicants;
using CareerTech.Response.Provinces;
using CareerTech.Service.Interfaces;


namespace CareerTech.Service.Services;

public class ProvinceService : IProvinceService
{
    private readonly IProvincesRepo provinceRepo;
    private readonly IUserRepo userRepo;
    private readonly IProvincesJdPostRepo jdProvinceRepo;
    private readonly IApplicantProvinceRepo applicantProvinceRepo;
    private readonly DatabaseContext databaseContext;

    public ProvinceService(
        IProvincesRepo provinceRepo,
        IProvincesJdPostRepo jdProvinceRepo,
        IApplicantProvinceRepo applicantProvinceRepo,
        IUserRepo userRepo,
        DatabaseContext databaseContext)
    {
        this.provinceRepo = provinceRepo;
        this.jdProvinceRepo = jdProvinceRepo;
        this.applicantProvinceRepo = applicantProvinceRepo;
        this.userRepo = userRepo;
        this.databaseContext = databaseContext;
    }

    public async Task<IList<Province>> GetProvinces()
    {
        return await this.provinceRepo.FindManyAsync(us => true);
    }

    public async Task<IList<ProvinceResponseDto>> GetJdProvince(int? jdPostId)
    {
        var provinces = await this.GetProvinces();
        var jdProvinceDtos = new List<ProvinceResponseDto>();
        var jdProvinces = jdPostId != null ? await this.jdProvinceRepo.FindManyAsync(us => us.JdPostId == jdPostId) : new List<ProvinceJdPost>();

        foreach (var province in provinces)
        {
            bool selected = jdProvinces.Any(us => us.ProvinceId == province.Id);
            jdProvinceDtos.Add(new ProvinceResponseDto(province, selected));
        }

        return jdProvinceDtos;
    }

    public async Task<IList<ProvinceResponseDto>> GetApplicantProvinces(int userId)
    {
        var provinces = await this.GetProvinces();
        var applicantProvincesDto = new List<ProvinceResponseDto>();
        var applicantProvinces = await this.applicantProvinceRepo.FindManyAsync(us => us.UserId == userId);

        foreach(var province in provinces)
        {
            bool selected = applicantProvinces.Any(us => us.ProvinceId == province.Id);
            applicantProvincesDto.Add(new ProvinceResponseDto(province, selected));
        }

        return applicantProvincesDto;
    }

    public async Task<bool> UpdateApplicantProvince(UpdateApplicantProvinceDto requestDto)
    {
        var user = await this.userRepo.FindOneAsync(us => us.Id == requestDto.UserId);

        if (user == default)
        {
            throw new Exception("errUserNotFound");
        }

        var transaction = this.databaseContext.Database.BeginTransaction();

        try
        {
            if(requestDto.Provinces.Count == 0)
            {
                var removeProvinces = await this.applicantProvinceRepo.FindManyAsync(us => us.UserId == requestDto.UserId);
                this.applicantProvinceRepo.Remove(removeProvinces);
            }
            else
            {
                var olderProvinces = await this.applicantProvinceRepo.FindManyAsync(us => us.UserId == requestDto.UserId);
                var requestProvinces = await this.provinceRepo.FindManyAsync(us => requestDto.Provinces.Contains(us.Id));
                var removeProvinces = olderProvinces.ExceptBy(requestProvinces.Select(us => us.Id), uq => uq.ProvinceId).ToList();
                var addProvinces = requestProvinces.ExceptBy(olderProvinces.Select(us => us.ProvinceId), uq => uq.Id).ToList();

                if(removeProvinces.Count != 0)
                {
                    this.applicantProvinceRepo.Remove(removeProvinces);
                }


                if(addProvinces.Count != 0)
                {
                    var applicantProvinces = addProvinces.Select(province => new ApplicantProvince
                    {
                        ProvinceId = province.Id,
                        UserId = requestDto.UserId,
                    });

                    await this.applicantProvinceRepo.SaveManyAsync(applicantProvinces);
                }
            }

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
