using CareerTech.Model.Entities;
using CareerTech.Request.Applicants;
using CareerTech.Response.Provinces;

namespace CareerTech.Service.Interfaces;

public interface IProvinceService
{
    Task<IList<Province>> GetProvinces();

    Task<IList<ProvinceResponseDto>> GetJdProvince(int? jdPostId);

    Task<IList<ProvinceResponseDto>> GetApplicantProvinces(int userId);

    Task<bool> UpdateApplicantProvince(UpdateApplicantProvinceDto requestDto);
}
