using CareerTech.Common.Utils;
using CareerTech.Model.Entities;
using CareerTech.Model.Enums;
using CareerTech.Request.Recruitments;
using System.Net.NetworkInformation;

namespace CareerTech.Mapper;

public static class JdPostMapper
{
    public static JdPost ToCreateModel(AddRecruiterJdDto requestDto)
    {
        return new JdPost
        {
            Title = requestDto.Title,
            UserId = requestDto.UserId,
            DomainId = requestDto.DomainId,
            Description = requestDto.Description,
            EndDate = !string.IsNullOrEmpty(requestDto.EndDate) ? Helper.ConvertStringToDateTime(requestDto.EndDate) : null,
            MinSalary = !string.IsNullOrEmpty(requestDto.MinSalary) ? Convert.ToDouble(requestDto.MinSalary) : null,
            MaxSalary = !string.IsNullOrEmpty(requestDto.MaxSalary) ? Convert.ToDouble(requestDto.MaxSalary) : null,
            CurrencySalary = requestDto.CurrencySalary <= 0 ? null : requestDto.CurrencySalary,
            WorkingType = requestDto.WorkingType,
            Quantity = !string.IsNullOrEmpty(requestDto.Quantity) ? Convert.ToInt32(requestDto.Quantity) : null,
            ExperienceYear = requestDto.ExperienceYear,
            Status = requestDto.Status,
            Requirement = requestDto.Requirement,
            Benefits = requestDto.Benefit,
            WorkingHours = requestDto.WorkingHour,
            WorkLocation = requestDto.WorkLocation,
            IsDelete = EDelete.NoDelete
        };
    }

    public static JdPost ToUpdateModel(UpdateRecruiterJdDto requestDto, JdPost jdPost)
    {
        jdPost.Title = requestDto.Title;
        jdPost.DomainId = requestDto.DomainId;
        jdPost.Description = requestDto.Description;
        jdPost.EndDate = !string.IsNullOrEmpty(requestDto.EndDate) ? Helper.ConvertStringToDateTime(requestDto.EndDate) : null;
        jdPost.MinSalary = !string.IsNullOrEmpty(requestDto.MinSalary) ? Convert.ToDouble(requestDto.MinSalary) : null;
        jdPost.MaxSalary = !string.IsNullOrEmpty(requestDto.MaxSalary) ? Convert.ToDouble(requestDto.MaxSalary) : null;
        jdPost.CurrencySalary = requestDto.CurrencySalary;
        jdPost.WorkingType = requestDto.WorkingType;
        jdPost.Quantity = !string.IsNullOrEmpty(requestDto.Quantity) ? Convert.ToInt32(requestDto.Quantity) : null;
        jdPost.ExperienceYear = requestDto.ExperienceYear;
        jdPost.Status = requestDto.Status;
        jdPost.Requirement = requestDto.Requirement;
        jdPost.Benefits = requestDto.Benefit;
        jdPost.WorkingHours = requestDto.WorkingHour;
        jdPost.WorkLocation = requestDto.WorkLocation;

        return jdPost;
    }
}
