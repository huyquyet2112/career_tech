using CareerTech.Common.Utils;
using CareerTech.Model.Entities;
using CareerTech.Request.Recruitments;
namespace CareerTech.Mapper;

public static class RecruiterMapper
{
    public static RecruiterDetail ToUpdateModel(RecruiterDetail recruiter, UpdateRecruiterBasicInfoDto requestDto)
    {
        recruiter.Name = requestDto.Name;
        recruiter.Avatar = requestDto.Avatar;
        recruiter.PhoneNumber = requestDto.PhoneNumber;
        recruiter.Address = requestDto.Address;
        recruiter.EstablishedDate = !string.IsNullOrEmpty(requestDto.EstablishedDate) ? Helper.ConvertStringToDateTime(requestDto.EstablishedDate) : null;
        recruiter.LinkWeb = requestDto.LinkWeb;
        recruiter.Description = requestDto.Description;
        recruiter.LocationMap = requestDto.LocationMap;
        recruiter.WorkSchedule = requestDto.WorkSchedule < 0 ? null : requestDto.WorkSchedule;
        recruiter.CompanySize = requestDto.CompanySize < 0 ? null : requestDto.CompanySize;
        return recruiter;
    }
}
