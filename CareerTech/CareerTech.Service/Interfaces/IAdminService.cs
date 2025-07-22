using CareerTech.Model.Entities;
using CareerTech.Request.Admins;
using CareerTech.Request.JobPosts;
using CareerTech.Response.Admins;
using CareerTech.Response.Applicants;
using CareerTech.Response.JobPosts;
using CareerTech.Response.Recruitments;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CareerTech.Service.Interfaces;

public interface IAdminService
{
    Task<IList<JobPostAdminResponseDto>> GetJobPostsForAdmin();

    Task<bool> CreateJobPostApproval(AddJobPostApprovalDto requestDto);

    Task<AdminDetail> Detail(int userId);

    Task<bool> UpdateAdminDetail(UpdateAdminDetailDto requestDto, string imagePathFolder, int userId);

    Task<IList<RecruiterPublicDto>> GetAdminRecruiters(QueryUserDto query);

    Task<IList<ApplicantAdminDto>> GetAdminApplicants(QueryUserDto query);

    Task<StatusUserDto> GetStatusUser(int userId);

    Task<bool> UpdateStatusUser(UpdateStatusUserDto requestDto, int userId);

    Task<ViewJobPostPaginationDto> GetJobPostByRecruiterForAdmin(QueryListJobPostDto query, int userId);

    Task<IList<UserRoleStatisticDto>> GetUserRoleStatisticDtos();

    Task<IList<JobStatusStatisticDto>> GetJobStatusStatisticDtos();

    Task<IList<Role>> GetRoles();

    Task<Role> RoleDetail(int roleId);

    Task<IList<RolePermissionResponseDto>> GetRolePermissions(int roleId);

    Task<IList<RolePermission>> UpdateRolePermissions(UpdateRolePermissionDto requestDto);
}
