using CareerTech.Model.Entities;

namespace CareerTech.Response.Recruitments;

/// <summary>
/// GroupDomainRecruitmentDto.
/// </summary>
/// <param name="groupDomain">represent for GroupDomain.</param>
/// <param name="isSelected">isSelected.</param>
public class GroupDomainRecruiterDto(GroupDomain groupDomain, bool isSelected = false)
{
    /// <summary>
    /// Gets or sets Id.
    /// </summary>
    public int Id { get; set; } = groupDomain.Id;

    public string Name { get; set; } = groupDomain.Name;

    /// <summary>
    /// Gets or sets isSelected.
    /// </summary>
    public bool IsSelected { get; set; } = isSelected;
}
