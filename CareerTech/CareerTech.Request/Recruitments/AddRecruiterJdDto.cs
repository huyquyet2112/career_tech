using CareerTech.Model.Enums;
using CareerTech.Request.Errors;

namespace CareerTech.Request.Recruitments;

public class AddRecruiterJdDto
{
    required public string Title { get; set; }

    required public int DomainId { get; set; }

    public List<int> Levels { get; set; } = new List<int>();

    required public EWorkingType WorkingType { get; set; }

    public List<int> Provinces { get; set; } = new List<int>(); 

    public List<int> Skills { get; set; } = new List<int>();

    public EJdPostStatus Status { get; set; }

    public string? MinSalary { get; set; }

    public string? MaxSalary { get; set; }

    public ECurrencySalary CurrencySalary { get; set; }

    public string? EndDate { get; set; }

    public string? Quantity { get; set; }

    required public EExperienceYear ExperienceYear { get; set; }

    public string? Description { get; set; }

    public string? Requirement { get; set; }

    public string? Benefit { get; set; }

    public string? WorkLocation { get; set; }

    public string? WorkingHour { get; set; }

    required public int UserId { get; set; }

    public List<ErrorValidateDto> Validate()
    {
        var errors = new List<ErrorValidateDto>();

        if (this.Title == null)
        {
            errors.Add(new ErrorValidateDto { Name = "title", Error = "errTitleCannotBeEmpty" });
        }

        if (this.DomainId < 0)
        {
            errors.Add(new ErrorValidateDto { Name = "domain", Error = "errDomainCannotBeEmpty" });
        }

        if (this.Levels.Count == 0)
        {
            errors.Add(new ErrorValidateDto { Name = "level", Error = "errLevelCannotBeEmpty" });
        }

        if (this.WorkingType < 0)
        {
            errors.Add(new ErrorValidateDto { Name = "workingType", Error = "errWorkingTypeCannotBeEmpty" });
        }

        if (this.Provinces.Count == 0)
        {
            errors.Add(new ErrorValidateDto { Name = "province", Error = "errProvinceCannotBeEmpty" });
        }

        if(this.Status < 0)
        {
            errors.Add(new ErrorValidateDto { Name = "status", Error = "errStatusCannotBeEmpty" });
        }

        if (this.MinSalary != null && this.MaxSalary != null)
        {
            if (Convert.ToInt32(this.MinSalary) > Convert.ToInt32(this.MaxSalary))
            {
                errors.Add(new ErrorValidateDto { Name = "minSalary", Error = "errMinSalaryCannotBeGreaterThanMaxSalary" });
            }
        }

        if (this.MinSalary != null || this.MaxSalary != null)
        {
            if(this.CurrencySalary < 0)
            {
                errors.Add(new ErrorValidateDto { Name = "currencySalary", Error = "errCurrencySalaryCannotBeEmpty" });
            }
        }

        if(this.CurrencySalary > 0)
        {
            if(this.MinSalary == null && this.MaxSalary == null)
            {
                errors.Add(new ErrorValidateDto { Name = "currencry", Error = "errMinSalaryorMaxSalaryCannotBeEmpty" });
            }
        }

        if(this.EndDate == null)
        {
            errors.Add(new ErrorValidateDto { Name = "endDate", Error = "errEndDateCannotBeEmpty" });
        }

        if(this.Quantity == null)
        {
            errors.Add(new ErrorValidateDto { Name = "quantity", Error = "errQuantityCannotBeEmpty" });
        }

        if(this.ExperienceYear < 0)
        {
            errors.Add(new ErrorValidateDto { Name = "experienceYear", Error = "errExperienceYearCannotBeEmpty" });
        }

        if(this.Description == null)
        {
            errors.Add(new ErrorValidateDto { Name = "description", Error = "errDescriptionCannotBeEmpty" });
        }

        if(this.Requirement == null)
        {
            errors.Add(new ErrorValidateDto { Name = "requirement", Error = "errRequirementCannotBeEmpty" });
        }

        if(this.Benefit == null)
        {
            errors.Add(new ErrorValidateDto { Name = "benefit", Error = "errBenefitCannotBeEmpty" });
        }

        if(this.WorkLocation == null)
        {
            errors.Add(new ErrorValidateDto { Name = "workLocation", Error = "errWorkLocationCannotBeEmpty" });
        }

        return errors;
    }
}
