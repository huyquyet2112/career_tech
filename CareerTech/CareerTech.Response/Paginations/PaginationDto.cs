namespace CareerTech.Response.Paginations;

public class PaginationDto(int page, int pageSize, int totalRecord)
{
    public int Page { get; set; } = page;

    public int PageSize { get; set; } = pageSize;

    public int TotalRecord { get; set; } = totalRecord;

    public int TotalPage { get; set; } = (int)Math.Ceiling((double)totalRecord / pageSize);
}
