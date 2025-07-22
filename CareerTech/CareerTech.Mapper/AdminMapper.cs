using CareerTech.Model.Entities;
using CareerTech.Request.Admins;

namespace CareerTech.Mapper;

public static class AdminMapper
{
    public static AdminDetail ToUpdateModel(AdminDetail admin, UpdateAdminDetailDto requestDto)
    {
        admin.Avatar = requestDto.Avatar;
        admin.Name = requestDto.Name;

        return admin;
    }
}
