using CareerTech.Response.Paginations;

namespace CareerTech.Response.Applicants;

public class ViewSaveJobPostDto(IEnumerable<ApplicantSavedJobPostDto> saveJobPost, PaginationDto pagination)
{
    public IEnumerable<ApplicantSavedJobPostDto> SavedJobPost { get; set; } = saveJobPost;

    public PaginationDto Pagination { get; set; } = pagination;
}
