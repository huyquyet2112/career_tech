using CareerTech.Response.Paginations;

namespace CareerTech.Response.JobPosts;

public class ViewJobPostPaginationDto(IEnumerable<JobPostResponseDto> jobPosts, PaginationDto pagination)
{
    public IEnumerable<JobPostResponseDto> JobPost { get; set; } = jobPosts;

    public PaginationDto Pagination { get; set; } = pagination;
}
