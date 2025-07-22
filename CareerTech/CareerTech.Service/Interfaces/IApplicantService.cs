using CareerTech.Model.Entities;
using CareerTech.Request.Applicants;
using CareerTech.Response.Applicants;

namespace CareerTech.Service.Interfaces;

public interface IApplicantService
{
    Task<ApplicantDetail> Detail(int userId);

    Task<IList<CvFile>> ApplicantCvFiles(int userId);

    Task<bool> UpdateApplicantBasicInfo(UpdateApplicantBasicInforDto requestDto, string imagePathFolder, int userId);

    Task<bool> ApplyJob(ApplyJobDto requestDto);

    Task<ViewApplicationJobPostDto> GetAppliedJobPosts(QueryApplicationJobPost query, int userId);

    Task<ViewSaveJobPostDto> GetSavedJobPost(QueryApplicationJobPost query, int userId);

    Task<bool> DeleteSavedJob(int savedId);
}
