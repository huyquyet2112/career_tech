using CareerTech.Model.Enums;
using CareerTech.Request.Errors;
using Microsoft.Identity.Client;
using System.Globalization;

namespace CareerTech.Request.Applicants;

public class UpdateApplicantBasicInforDto
{
    public string? Avatar {  get; set; }

    public string? OldAvatar { get; set; }

    required public string Name { get; set; }

    public string? PhoneNumber { get; set; }

    required public EGender Gender { get; set; }

    public string? Birthday { get; set; }

    public string? Address { get; set; }

    public string? Description { get; set; }

    public List<ErrorValidateDto> Validate()
    {
        var errors = new List<ErrorValidateDto>();

        if (this.Name == null)
        {
            errors.Add(new ErrorValidateDto { Name = "name", Error = "errNameCannotEmpty" });
        }

        if(this.Gender < 0)
        {
            errors.Add(new ErrorValidateDto { Name = "gender", Error = "errGenderCannotEmpty" });
        }

        if (!string.IsNullOrWhiteSpace(this.Birthday) && !DateTime.TryParseExact(this.Birthday, "yyyy/MM/dd",
        CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            errors.Add(new ErrorValidateDto
            {
                Name = "birthday",
                Error = "errBirthdayInvalidFormat"
            });
        }

        return errors;
    }
}
