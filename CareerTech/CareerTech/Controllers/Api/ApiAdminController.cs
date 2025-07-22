using CareerTech.Request.Admins;
using CareerTech.Request.Errors;
using CareerTech.Request.JobPosts;
using CareerTech.Request.Recruitments;
using CareerTech.Response.Apis;
using CareerTech.Service.Interfaces;
using CareerTech.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CareerTech.Controllers.Api;

[Route("api/admin")]
public class ApiAdminController : BaseController
{
    private readonly ILogger<ApiAdminController> logger;
    private readonly IStringLocalizer<Language> localizer;
    private readonly IAdminService adminService;
    private readonly IJdPostService jobPostService;
    private readonly IWebHostEnvironment environment;

    public ApiAdminController(
        ILogger<ApiAdminController> logger,
        IStringLocalizer<Language> localizer,
        IAdminService adminService,
        IJdPostService jobPostService,
        IWebHostEnvironment environment) : base(localizer)
    {
        this.logger = logger;
        this.localizer = localizer;
        this.adminService = adminService;
        this.jobPostService = jobPostService;
        this.environment = environment;
    }

    [HttpGet]
    [Route("job-posts")]
    public async Task<IActionResult> GetJobPosts()
    {
        var result = await this.adminService.GetJobPostsForAdmin();
        return this.Ok(result);
    }

    [HttpGet]
    [Route("recruiters")]
    public async Task<IActionResult> GetRecruiters(QueryUserDto query)
    {
        var result = await this.adminService.GetAdminRecruiters(query);
        return this.Ok(result); 
    }

    [HttpGet]
    [Route("applicants")]
    public async Task<IActionResult> GetApplicants(QueryUserDto query)
    {
        var result = await this.adminService.GetAdminApplicants(query);
        return this.Ok(result);
    }

    [HttpGet]
    [Route("user-status/{userId}")]
    public async Task<IActionResult> GetUserStatus(int userId)
    {
        var result = await this.adminService.GetStatusUser(userId);
        return this.Ok(result);
    }

    [HttpPut]
    [Route("user-status/{userId}")]
    public async Task<IActionResult> UpdateStatusUser([FromBody] UpdateStatusUserDto requestDto, int userId)
    {
        var result = await this.adminService.UpdateStatusUser(requestDto, userId);

        if (result)
        {
            return this.Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateStatusSuccess", this.Localizer)));
        }
        else
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateStatusFailed", this.Localizer)));
        }
    }

    [HttpPost]
    [Route("job-post-approvals")]
    public async Task<IActionResult> CreateJobPostApproval([FromBody] AddJobPostApprovalDto requestDto)
    {
        var errors = requestDto.Validate();

        try
        {
            if (errors.Any())
            {
                var errorResponse = new List<ErrorValidateDto>();

                foreach (var error in errors)
                {
                    var message = new MessageResponseDto(Utils.GetLocalizedMessage($"{error.Error}", this.Localizer));
                    errorResponse.Add(new ErrorValidateDto
                    {
                        Name = error.Name,
                        Error = message.Message,
                    });
                }

                return this.BadRequest(errorResponse);
            }

            var result = await this.adminService.CreateJobPostApproval(requestDto);

            if (!result)
            {
                return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgCreateJobPostApprovalFailed", this.localizer)));
            }

            return this.Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgCreateJobPostApprovalSuccess", this.localizer)));
        }
        catch
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgCreateJobPostApprovalFailed", this.localizer)));
        }
    }

    [HttpPut]
    [Route("job-post-approvals")]
    public async Task<IActionResult> UpdateJobPostApproval([FromBody] UpdateJobPostApprovalDto requestDto)
    {
        var errors = requestDto.Validate();

        try
        {
            if (errors.Any())
            {
                var errorResponse = new List<ErrorValidateDto>();

                foreach (var error in errors)
                {
                    var message = new MessageResponseDto(Utils.GetLocalizedMessage($"{error.Error}", this.Localizer));
                    errorResponse.Add(new ErrorValidateDto
                    {
                        Name = error.Name,
                        Error = message.Message,
                    });
                }

                return this.BadRequest(errorResponse);
            }

            var result = await this.jobPostService.UpdateJobPostApproval(requestDto);

            if (!result)
            {
                return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateJobPostApprovalFailed", this.localizer)));
            }

            return this.Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateJobPostApprovalSuccess", this.localizer)));
        }
        catch
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgCreateJobPostApprovalFailed", this.localizer)));
        }
    }

    [HttpPut]
    [Route("{userId}/basic-info")]
    public async Task<IActionResult> UpdateAdminDetail([FromBody] UpdateAdminDetailDto requestDto, int userId)
    {
        try
        {
            var imagePathFolder = Path.Combine(this.environment.WebRootPath);

            var validate = requestDto.Validate();

            if (validate.Count != 0)
            {
                var errorResponse = new List<ErrorValidateDto>();

                foreach (var error in validate)
                {
                    var message = new MessageResponseDto(Utils.GetLocalizedMessage($"{error.Error}", this.Localizer));
                    errorResponse.Add(new ErrorValidateDto
                    {
                        Name = error.Name,
                        Error = message.Message,
                    });
                }

                return this.BadRequest(errorResponse);
            }

            var updateAdmin = await this.adminService.UpdateAdminDetail(requestDto, imagePathFolder, userId);

            if (updateAdmin)
            {
                return this.Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateAdminSuccess", this.Localizer)));
            }
            else
            {
                return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateAdminFailed", this.Localizer)));
            }
        }
        catch (Exception ex)
        {
            return this.BadRequest(Utils.GetLocalizedMessage($"{ex}", this.localizer));
        }
    }

    [HttpGet]
    [Route("roles")]
    public async Task<IActionResult> GetRoles()
    {
        var result  = await this.adminService.GetRoles();

        return this.Ok(result);
    }

    [HttpPut]
    [Route("role-permissions")]
    public async Task<IActionResult> UpdateRolePermissions([FromBody] UpdateRolePermissionDto requestDto)
    {
        try
        {
            var result = await this.adminService.UpdateRolePermissions(requestDto);

            return this.Ok(new
            {
                Message = new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateRolePermissionSuccess", this.Localizer)),
                RolePermissions = result,
            });
        }
        catch (Exception)
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage($"msgUpdateRolePermissionFailed", this.Localizer)));
        }
    }
}
