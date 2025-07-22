namespace CareerTech.Response.Applicants;

public class QueryApplicationJobPost
{
    required public int Page { get; set; } = 1;

    required public int PageSize { get; set; } = 5;

    public int GetSkip()
    {
        if (this.Page <= 0)
        {
            return 0;
        }

        return (this.Page - 1) * this.PageSize;
    }
}
