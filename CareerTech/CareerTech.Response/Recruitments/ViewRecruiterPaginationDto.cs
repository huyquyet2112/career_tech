using CareerTech.Response.Paginations;

namespace CareerTech.Response.Recruitments;

public class ViewRecruiterPaginationDto(IEnumerable<RecruiterPublicDto> recruiters, PaginationDto pagination)
{
    public IEnumerable<RecruiterPublicDto> Recruiters { get; set; } = recruiters;

    public PaginationDto Pagination { get; set; } = pagination;
}
