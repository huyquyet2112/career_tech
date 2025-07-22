using CareerTech.Model.Enums;

namespace CareerTech.Response.Admins;

public class UserRoleStatisticDto
{
    required public EUserRole Role { get; set; }

    public int Count { get; set; }
}
