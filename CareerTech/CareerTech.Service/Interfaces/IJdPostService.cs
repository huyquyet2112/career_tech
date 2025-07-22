using CareerTech.Model.Entities;
using CareerTech.Request.JobPosts;
using CareerTech.Response.JobPosts;
using CareerTech.Response.Recruitments;


namespace CareerTech.Service.Interfaces;

public interface IJdPostService
{
    Task<JdPost> GetJobPostById(int id);

    Task<IList<JobPostStatusApprovalDto>> GetJobApprovalsById(int jobPostId);

    Task<JdPostApproval> GetJobApprovalById(int jobPostApprovalId);

    Task<bool> UpdateJobPostApproval(UpdateJobPostApprovalDto requestDto);

    Task<ViewJobPostPaginationDto> GetJobPostPublic(QueryListJobPostDto query);

    Task<JobPostPublicDetailDto> GetDetailJobPost(int jobPostId, int userId);

    Task<IList<JobPostApplicantsDto>> GetJobPostApplicants(int jobPostId);

    Task<bool> UpdateStatusApplication(int applyId);

    Task<JobPostApplyResponseDto> GetDetailApplicantion(int applyId);

    Task<bool> UpdateFitStatusApplication(JobPostAppicantionFitStatusDto requestDto, int applyId);

    Task<bool> UpdateSavedJob(UpdateSavedJobDto requestDto);

    Task<List<JobPostResponseDto>> GetJobPostPulicIndex();

    Task<int> GetRecruiterIdByJobId(int jobJostId);
}
