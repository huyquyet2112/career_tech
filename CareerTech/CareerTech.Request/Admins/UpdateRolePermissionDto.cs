namespace CareerTech.Request.Admins;

public class UpdateRolePermissionDto
{
    required public int RoleId { get; set; }

    required public List<AddPermissionDto> Permissions { get; set; } = [];

    public IEnumerable<int> GetPermissionIds()
    {
        return this.Permissions.Select(us => us.Id);
    }
}
