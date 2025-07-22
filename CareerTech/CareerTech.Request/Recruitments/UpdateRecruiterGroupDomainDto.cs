namespace CareerTech.Request.Recruitments;

/// <summary>
/// UpdateRecruitmentGroupDomainDto.
/// </summary>
public class UpdateRecruiterGroupDomainDto
{
    /// <summary>
    /// Gets or sets UserId.
    /// </summary>
    required public int UserId { get; set; }

    required public List<RecruiterGroupDomainDto> GroupDomains { get; set; } = [];

    public IEnumerable<int> GetGroupDomainIds()
    {
        return this.GroupDomains.Select(us => us.GroupDomainId);
    }
}
