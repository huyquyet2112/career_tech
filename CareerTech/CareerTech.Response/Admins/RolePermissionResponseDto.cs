using CareerTech.Model.Entities;

namespace CareerTech.Response.Admins;

public class RolePermissionResponseDto(Permission permission, bool isSelected = false)
{
    public int Id { get; set; } = permission.Id;

    public string Name { get; set; } = permission.Name;

    public string Controller { get; set; } = permission.Controller;

    public string Action { get; set; } = permission.Action;

    public string Method { get; set; } = permission.Method;

    public string Url { get; set; } = permission.Url;

    public bool IsSelected { get; set; } = isSelected;
}
