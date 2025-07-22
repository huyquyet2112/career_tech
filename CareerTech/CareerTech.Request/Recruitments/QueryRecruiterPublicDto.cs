namespace CareerTech.Request.Recruitments;

public class QueryRecruiterPublicDto
{
    required public int Page { get; set; } = 1;

    required public int PageSize { get; set; } = 6;

    public string? Name { get; set; }

    public List<int>? GroupDomains { get; set; }

    public bool IsSelectedDomain(int groupDomainId)
    {
        if(this.GroupDomains == null)
        {
            return false;
        }

        return this.GroupDomains.Contains(groupDomainId);
    }

    public int GetSkip()
    {
        if (this.Page <= 0)
        {
            return 0;
        }

        return (this.Page - 1) * this.PageSize;
    }
}
