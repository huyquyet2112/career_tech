using CareerTech.Model.Entities;
using CareerTech.Request.Applicants;
using CareerTech.Request.JobPosts;
using CareerTech.Request.Recruitments;
using CareerTech.Response.Admins;
using CareerTech.Response.Applicants;
using CareerTech.Response.JobPosts;
using CareerTech.Response.Recruitments;

namespace CareerTech.Service.Interfaces;

public interface IRecruiterService
{
    /// <summary>
    /// Get RecruitmentDetail.
    /// </summary>
    /// <param name="userId">parameter.</param>
    /// <returns>RecruitmentDetail.</returns>
    Task<RecruiterDetail> GetRecruiterDetail(int userId);

    Task<IList<GroupDomainRecruiterDto>> GetGroupDomainRecruiter(int userId);

    Task<IList<RecruiterDomain>> UpdateRecruiterGroupDomain(UpdateRecruiterGroupDomainDto requestDto);

    Task<bool> UpdateRecruiterBasicInfo(UpdateRecruiterBasicInfoDto requestDto, string imagePathFolder, int userId);

    Task<IList<RecruiterJobPostListDto>> GetRecruiterJobPostsAsync(int userId);

    Task<bool> CreateJobPost(AddRecruiterJdDto requestDto);

    Task<bool> UpdateJobPost(UpdateRecruiterJdDto requestDto);

    Task<ViewJobPostPaginationDto> GetJobPostPublicByRecruiter(QueryListJobPostDto query, int userId);

    Task<IList<RecruiterPublicDto>> GetRecruitersIndex();

    Task<ViewRecruiterPaginationDto> GetRecruiters(QueryRecruiterPublicDto query);

    Task<IList<JobStatusStatisticDto>> GetJobStatusStatisticByRecruiter(int userId);

    Task<IList<ApplicantAdminDto>> GetApplicantsPublic(QueryApplicantPublicDto query);
}
