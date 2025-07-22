using CareerTech.Response.JobPosts;
using CareerTech.Response.Paginations;

namespace CareerTech.Response.Applicants;

public class ViewApplicationJobPostDto(IEnumerable<ApplicantAppliedJobPostDto> applicationJobPost, PaginationDto pagination)
{
    public IEnumerable<ApplicantAppliedJobPostDto> ApplicationJobPost { get; set; } = applicationJobPost;

    public PaginationDto Pagination { get; set; } = pagination;
}
