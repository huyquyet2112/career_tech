
using CareerTech.Model.Entities;
using CareerTech.Model.Enums;
using CareerTech.Request.Errors;

namespace CareerTech.Request.Recruitments;

/// <summary>
/// UpdateRecruitmentBasicInfoDto.
/// </summary>
public class UpdateRecruiterBasicInfoDto
{
    /// <summary>
    /// Gets or sets Image.
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// Gets or sets OldImage.
    /// </summary>
    public string? OldAvatar { get; set; }

    /// <summary>
    /// Gets or sets Name.
    /// </summary>
    required public string Name { get; set; }

    /// <summary>
    /// Gets or sets PhoneNumber.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets Address.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets EstablishedDate.
    /// </summary>
    public string? EstablishedDate { get; set; }

    /// <summary>
    /// Gets or sets LinkWeb.
    /// </summary>
    public string? LinkWeb { get; set; }

    /// <summary>
    /// Gets or sets Description.
    /// </summary>
    public string? Description { get; set; }


    /// <summary>
    /// Location
    /// </summary>
    public string? LocationMap { get; set; }

    public EWorkSchedule WorkSchedule { get; set; }

    public ECompanySize CompanySize { get; set; }

    public List<ErrorValidateDto> Validate()
    {
        var errors = new List<ErrorValidateDto>();

        if (this.Name == null)
        {
            errors.Add(new ErrorValidateDto { Name = "name", Error = "errNameCannotEmpty" });
        }

        if(this.CompanySize < 0)
        {
            errors.Add(new ErrorValidateDto { Name = "companySize", Error = "errCompanySizeCannotBeEmpty" });
        }

        if(this.WorkSchedule < 0)
        {
            errors.Add(new ErrorValidateDto { Name = "workSchedule", Error = "errWorkScheduleCannotBeEmpty" });
        }

        return errors;
    }
}
