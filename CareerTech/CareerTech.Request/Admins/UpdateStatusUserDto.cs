using CareerTech.Model.Enums;
namespace CareerTech.Request.Admins;

public class UpdateStatusUserDto
{
    required public EUserStatus Status { get;set; }

    required public EVerifyStatus VerifyStatus { get;set; }
}
