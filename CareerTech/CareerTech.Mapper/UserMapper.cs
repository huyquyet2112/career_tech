using CareerTech.Model.Entities;
using CareerTech.Model.Enums;
using CareerTech.Request.Authentication;
using BC = BCrypt.Net.BCrypt;

namespace CareerTech.Mapper;

public static class UserMapper
{
    public static User ToCreateModel(RegisterDto registerDto)
    {
        return new User
        {
            Role = registerDto.Role,
            UserName = registerDto.UserName,
            HashPassword = BC.HashPassword(registerDto.Password),
            Status = EUserStatus.Active,
            VerifyStatus = registerDto.Role == EUserRole.Recruitment ? EVerifyStatus.Pending : EVerifyStatus.Approved
        };
    }
}
