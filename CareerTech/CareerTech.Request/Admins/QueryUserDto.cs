using CareerTech.Model.Enums;

namespace CareerTech.Request.Admins;

public class QueryUserDto
{
    public List<EUserStatus>? Status { get; set; }

    public string? Name { get; set; }

    public bool IsSelectedStatus(EUserStatus status)
    {
        if(this.Status == null)
        {
            return false;
        }

        return this.Status.Contains(status);
    }
}
