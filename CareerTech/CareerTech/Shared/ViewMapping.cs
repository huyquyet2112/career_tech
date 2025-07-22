using System.Text;

namespace CareerTech.Shared;

public class ViewMapping
{
    // Folder.
    private static string Views { get; } = "~/Views";

    private static string ViewAdmin { get; } = $"{Views}/Admin";

    private static string ViewApplicant { get; } = $"{Views}/Applicant";

    private static string ViewRecruiter { get; } = $"{Views}/Recruiter";

    private static string ViewAuthentication { get; } = $"{Views}/Authentication";

    private static string ViewError { get; } = $"{Views}/Error";

    private static string ViewShared { get; } = $"{Views}/Shared";

    private static string ViewSharedComponent { get; } = $"{ViewShared}/Components";

    // Authentication
    public static string ViewAdminLogin { get; } = $"{ViewAuthentication}/AdminLogin.cshtml";

    public static string ViewApplicantLogin { get; } = $"{ViewAuthentication}/ApplicantLogin.cshtml";

    public static string ViewRecruiterLogin { get; } = $"{ViewAuthentication}/RecruiterLogin.cshtml";

    public static string ViewRegister { get; } = $"{ViewAuthentication}/Register.cshtml";

    // Admin
    public static string ViewLayoutAdmin { get; } = $"{ViewAdmin}/_LayoutAdmin.cshtml";

    public static string ViewAdminIndex { get; } = $"{ViewAdmin}/Index.cshtml";

    public static string ViewAdminDashboard { get; } = $"{ViewAdmin}/Dashboard.cshtml";

    public static string ViewAdminJobPost { get; } = $"{ViewAdmin}/JobPost.cshtml";

    public static string ViewAdminDetailJobPost { get; } = $"{ViewAdmin}/DetailJobPost.cshtml";

    public static string ViewAdminDetail { get; } = $"{ViewAdmin}/Detail.cshtml";

    public static string ViewAdminRecruiter { get; } = $"{ViewAdmin}/Recruiter.cshtml";

    public static string ViewAdminApplicant { get; } = $"{ViewAdmin}/Applicant.cshtml";

    public static string ViewAdminApplicantDetail { get; } = $"{ViewAdmin}/ApplicantDetail.cshtml";

    public static string ViewAdminRecruiterDetail { get; } = $"{ViewAdmin}/RecruiterDetail.cshtml";

    public static string ViewAdminRole { get; } = $"{ViewAdmin}/Role.cshtml";

    public static string ViewAdminRoleDetail { get; } = $"{ViewAdmin}/RoleDetail.cshtml";

    // Applicant

    public static string ViewApplicantIndex { get; } = $"{ViewApplicant}/Index.cshtml";

    public static string ViewApplicantDetail { get; } = $"{ViewApplicant}/Detail.cshtml";

    public static string ViewApplicantAppliedJob { get; } = $"{ViewApplicant}/AppliedJob.cshtml";

    public static string ViewApplicantSavedJob { get; } = $"{ViewApplicant}/SavedJob.cshtml";

    public static string ViewJobsPublic { get; } = $"{ViewApplicant}/JobsPublic.cshtml";

    public static string ViewPublicDetailJobPost { get; } = $"{ViewApplicant}/DetailJobPost.cshtml";

    public static string ViewRecruitersPublic { get; } = $"{ViewApplicant}/RecruitersPublic.cshtml";

    public static string ViewRecruiterProfilePublic { get; } = $"{ViewApplicant}/ProfilePublic.cshtml";


    // Recruiter
    public static string ViewLayoutRecruiter { get; } = $"{ViewRecruiter}/_LayoutRecruiter.cshtml";

    public static string ViewRecruiterDetail { get; } = $"{ViewRecruiter}/Detail.cshtml";

    public static string ViewRecruiterDashboard { get; } = $"{ViewRecruiter}/Dashboard.cshtml";

    public static string ViewRecruiterJobPost { get; } = $"{ViewRecruiter}/JobPost.cshtml";

    public static string ViewJobPostApplicants { get; } = $"{ViewRecruiter}/JobPostApplicants.cshtml";

    public static string ViewRecruiterCRUJdPost { get; } = $"{ViewRecruiter}/Jd_CRU.cshtml";

    public static string ViewRecruiterIndex { get; } = $"{ViewRecruiter}/Index.cshtml";

    public static string ViewRecruiterApplicantPublic { get; } = $"{ViewRecruiter}/ApplicantPublic.cshtml";

    public static string ViewRecruiterApplicantDetail { get; } = $"{ViewRecruiter}/ApplicantDetail.cshtml";

    // Components
    public static string ViewDefaultAvatar { get; } = $"{ViewSharedComponent}/_DefaultAvatar.cshtml";

    public static string ViewLogout { get; } = $"{ViewSharedComponent}/_Logout.cshtml";

    public static string ViewChangePassword { get; } = $"{ViewSharedComponent}/_ChangePassword.cshtml";

    public static string ViewDefaultCompany { get; } = $"{ViewSharedComponent}/_DefaultCompany.cshtml";

    // Error

    public static string ViewForbiddenError { get; } = $"{ViewError}/Forbidden.cshtml";

    public static string ViewNotFoundError { get; } = $"{ViewError}/NotFound.cshtml";

    public static string ViewInternalServerError { get; } = $"{ViewError}/InternalServerError.cshtml";

}
