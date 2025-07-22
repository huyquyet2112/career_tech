using CareerTech.Model.Enums;

namespace CareerTech.Response.Admins;

public class StatusUserDto
{
    required public EUserStatus Status { get; set; }

    required public EVerifyStatus VerifyStatus { get; set; }
}
