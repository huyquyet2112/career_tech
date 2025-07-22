using CareerTech.Request.Errors;

namespace CareerTech.Request.Admins;

public class UpdateAdminDetailDto
{
    public string? Avatar { get; set; }

    public string? OldAvatar { get; set; }

    required public string Name { get; set; }

    public List<ErrorValidateDto> Validate()
    {
        var erros = new List<ErrorValidateDto>();

        if (this.Name == null)
        {
            erros.Add(new ErrorValidateDto { Name = "name", Error = "errNameCannotEmpty" });
        }

        return erros;
    }
}
