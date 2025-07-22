using Azure.Core;
using CareerTech.Model.Enums;
using CareerTech.Request.Admins;
using CareerTech.Request.JobPosts;
using CareerTech.Response.Apis;
using CareerTech.Service.Interfaces;
using CareerTech.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;


namespace CareerTech.Controllers;

[Route("admin")]
public class AdminController : BaseController
{
    private readonly ILogger<AdminController> logger;
    private readonly IStringLocalizer<Language> localizer;
    private readonly IRecruiterService recruiterService;
    private readonly ISkillService skillService;
    private readonly IApplicantService applicantService;
    private readonly ILevelService levelService;
    private readonly IDomainService domainService;
    private readonly IProvinceService provinceService;
    private readonly IJdPostService jdPostService;
    private readonly IAdminService adminService;

    public AdminController(
        ILogger<AdminController> logger,
        IStringLocalizer<Language> localizer,
        IRecruiterService recruiterService,
        ISkillService skillService,
        ILevelService levelService,
        IDomainService domainService,
        IProvinceService provinceService,
        IApplicantService applicantService,
        IAdminService adminService,
        IJdPostService jdPostService) : base(localizer)
    {
        this.logger = logger;
        this.localizer = localizer;
        this.recruiterService = recruiterService;
        this.skillService = skillService;
        this.levelService = levelService;
        this.domainService = domainService;
        this.provinceService = provinceService;
        this.applicantService = applicantService;
        this.jdPostService = jdPostService;
        this.adminService = adminService;
    }

    [HttpGet]
    [Route("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var userStatistic = await this.adminService.GetUserRoleStatisticDtos();
        var jobStatistic = await this.adminService.GetJobStatusStatisticDtos();

        this.ViewData["userStatistic"] = userStatistic;
        this.ViewData["jobStatistic"] = jobStatistic;
        return this.View(ViewMapping.ViewAdminDashboard);
    }

    [HttpGet]
    [Route("job-posts")]
    public IActionResult RenderViewJobPosts()
    {
        return this.View(ViewMapping.ViewAdminJobPost);
    }

    [HttpGet]
    [Route("recruiters")]
    public IActionResult RenderViewRecruiters(QueryUserDto query)
    {
        this.ViewData["query"] = query;
        return this.View(ViewMapping.ViewAdminRecruiter);
    }

    [HttpGet]
    [Route("applicants")]
    public IActionResult RenderViewApplicants(QueryUserDto query)
    {
        this.ViewData["query"] = query;
        return this.View(ViewMapping.ViewAdminApplicant);
    }

    [HttpGet]
    [Route("applicants/{userId}")]
    public async Task<IActionResult> DetailApplicant(int userId)
    {
        var applicant = await this.applicantService.Detail(userId);
        var applicantSkills = await this.skillService.GetApplicantGroupSkills(userId);
        var applicantCvFiles = await this.applicantService.ApplicantCvFiles(userId);
        var applicantLevels = await this.levelService.GetApplicantLevels(userId);
        var applicantProvinces = await this.provinceService.GetApplicantProvinces(userId);

        this.ViewData["applicantSkills"] = applicantSkills;
        this.ViewData["applicantCvFiles"] = applicantCvFiles;
        this.ViewData["applicantLevels"] = applicantLevels;
        this.ViewData["applicantProvinces"] = applicantProvinces;

        return this.View(ViewMapping.ViewAdminApplicantDetail, applicant);
    }

    [HttpGet]
    [Route("recruiters/{userId}")]
    public async Task<IActionResult> DetailRecruiter(QueryListJobPostDto query, int userId)
    {
        var recruiter = await this.recruiterService.GetRecruiterDetail(userId);
        var recruiterDomains = await this.recruiterService.GetGroupDomainRecruiter(userId);
        var jobPostPublicByRecruiter = await this.adminService.GetJobPostByRecruiterForAdmin(query, userId);
        var levels = await this.levelService.GetLevels();
        var provinces = await this.provinceService.GetProvinces();

        this.ViewData["levels"] = levels;
        this.ViewData["provinces"] = provinces;
        this.ViewData["recruiterDomains"] = recruiterDomains;
        this.ViewData["recruiterDetail"] = recruiter;
        this.ViewData["query"] = query;
        return this.View(ViewMapping.ViewAdminRecruiterDetail, jobPostPublicByRecruiter);
    }

    [HttpGet]
    [Route("job-posts/{jobPostId}")]
    public async Task<IActionResult> GetJobPostDetail(int jobPostId)
    {
        var jdSkill = await this.skillService.GetJdGroupSkillS(jobPostId);
        var jdLevel = await this.levelService.GetJdLevels(jobPostId);
        var jdProvince = await this.provinceService.GetJdProvince(jobPostId);
        var domains = await this.domainService.GetDomains();
        var jobPostApprovals = await this.jdPostService.GetJobApprovalsById(jobPostId);

        this.ViewData["jdSkill"] = jdSkill;
        this.ViewData["jdLevel"] = jdLevel;
        this.ViewData["jdProvince"] = jdProvince;
        this.ViewData["domains"] = domains;
        this.ViewData["jobPostApprovals"] = jobPostApprovals;

        var jdPost = await this.jdPostService.GetJobPostById(jobPostId);

        return this.View(ViewMapping.ViewAdminDetailJobPost, jdPost);
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<IActionResult> Detail(int userId)
    {
        var currentUserId = GetUserId();
        var currentUserRole = GetUserRole();

        if (currentUserRole == EUserRole.Admin.ToString())
        {
            if (currentUserId != userId)
            {
                return Redirect("/403");
            }
        }

        var adminDetail = await this.adminService.Detail(userId);

        return this.View(ViewMapping.ViewAdminDetail, adminDetail);
    }

    [HttpGet]
    [Route("roles")]
    public IActionResult RenderViewRoles()
    {
        return this.View(ViewMapping.ViewAdminRole);
    }

    [HttpGet]
    [Route("roles/{roleId}")]
    public async Task<IActionResult> RoleDetail(int roleId)
    {
        var role = await this.adminService.RoleDetail(roleId);
        var rolePermissions = await this.adminService.GetRolePermissions(roleId);

        this.ViewData["rolePermissions"] = rolePermissions;
        return this.View(ViewMapping.ViewAdminRoleDetail, role);
    }
}
