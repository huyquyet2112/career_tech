using CareerTech.Common.Utils;
using CareerTech.Model.Entities;
using CareerTech.Request.Applicants;

namespace CareerTech.Mapper;

public static class ApplicantDetailMapper
{
    public static ApplicantDetail ToUpdateModel(ApplicantDetail applicantDetail, UpdateApplicantBasicInforDto requestDto)
    {
        applicantDetail.Name = requestDto.Name;
        applicantDetail.Avatar = requestDto.Avatar;
        applicantDetail.Address = requestDto.Address;
        applicantDetail.Birthday = !string.IsNullOrEmpty(requestDto.Birthday) ? Helper.ConvertStringToDateTime(requestDto.Birthday) : null;
        applicantDetail.Description = requestDto.Description;
        applicantDetail.PhoneNumber = requestDto.PhoneNumber;
        applicantDetail.Gender = requestDto.Gender < 0 ? null : requestDto.Gender;

        return applicantDetail;
    }
}
