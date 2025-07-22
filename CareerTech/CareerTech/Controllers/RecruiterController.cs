using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using CareerTech.Shared;
using CareerTech.Service.Interfaces;
using CareerTech.Model.Entities;
using CareerTech.Model.Enums;
using CareerTech.Response.Recruitments;
using CareerTech.Request.JobPosts;
using System.Threading.Tasks;
using CareerTech.Request.Applicants;

namespace CareerTech.Controllers;

[Route("recruiters")]
public class RecruiterController : BaseController
{
    private readonly ILogger<RecruiterController> logger;
    private readonly IStringLocalizer<Language> localizer;
    private readonly IRecruiterService recruiterService;
    private readonly IProvinceService provinceService;
    private readonly ILevelService levelService;
    private readonly ISkillService skillService;
    private readonly IDomainService domainService;
    private readonly IJdPostService jdPostService;
    private readonly IApplicantService applicantService;

    public RecruiterController(
        ILogger<RecruiterController> logger,
        IStringLocalizer<Language> localizer,
        IRecruiterService recruiterService,
        IProvinceService provinceService,
        ILevelService levelService,
        ISkillService skillService,
        IDomainService domainService,
        IJdPostService jdPostService,
        IApplicantService applicantService
        ) : base(localizer)
    {
        this.logger = logger;
        this.localizer = localizer;
        this.recruiterService = recruiterService;
        this.provinceService = provinceService;
        this.levelService = levelService;
        this.skillService = skillService;
        this.domainService = domainService;
        this.jdPostService = jdPostService;
        this.applicantService = applicantService;
    }

    [HttpGet]
    [Route("dashboard")]
    public async Task<IActionResult> DashBoard()
    {
        var userId = GetUserId();
        var jobPostByRecruiter = await this.recruiterService.GetJobStatusStatisticByRecruiter(userId);

        this.ViewData["jobStatistic"] = jobPostByRecruiter;

        return this.View(ViewMapping.ViewRecruiterDashboard);
    }

    [HttpGet]
    [Route("/public-applicants")]
    public async Task<IActionResult> RenderViewApplicantsPublic(QueryApplicantPublicDto query)
    {
        var levels = await this.levelService.GetLevels();
        var provinces = await this.provinceService.GetProvinces();
        var skills = await this.skillService.GetSkills();

        this.ViewData["query"] = query;
        this.ViewData["levels"] = levels;
        this.ViewData["provinces"] = provinces;
        this.ViewData["skills"] = skills;

        return this.View(ViewMapping.ViewRecruiterApplicantPublic);
    }

    [HttpGet]
    [Route("public-applicants/{userId}")]
    public async Task<IActionResult> DetailPublicApplicant(int userId)
    {
        var applicant = await this.applicantService.Detail(userId);
        var applicantSkills = await this.skillService.GetApplicantGroupSkills(userId);
        var applicantLevels = await this.levelService.GetApplicantLevels(userId);
        var applicantProvinces = await this.provinceService.GetApplicantProvinces(userId);

        this.ViewData["applicantSkills"] = applicantSkills;
        this.ViewData["applicantLevels"] = applicantLevels;
        this.ViewData["applicantProvinces"] = applicantProvinces;

        return this.View(ViewMapping.ViewRecruiterApplicantDetail, applicant);
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<IActionResult> Detail(int userId)
    {
        var currentUserId = GetUserId();
        var currentUserRole = GetUserRole();

        if (currentUserRole == EUserRole.Recruitment.ToString())
        {
            if (currentUserId != userId)
            {
                return Redirect("/403");
            }
        }

        var recruitmentDetail = await this.recruiterService.GetRecruiterDetail(userId);
        var recruitmentsDomain = await this.recruiterService.GetGroupDomainRecruiter(userId);

        this.ViewData["recruitmentDomains"] = recruitmentsDomain;

        return this.View(ViewMapping.ViewRecruiterDetail, recruitmentDetail);
    }

    [HttpGet]
    [Route("{userId}/job-posts")]
    public IActionResult RenderViewRecruiterJobPosts(int userId)
    {
        var currentUserId = GetUserId();
        var currentUserRole = GetUserRole();

        if (currentUserRole == EUserRole.Recruitment.ToString())
        {
            if (currentUserId != userId)
            {
                return Redirect("/403");
            }
        }

        return this.View(ViewMapping.ViewRecruiterJobPost, userId);
    }

    [HttpGet]
    [Route("jd-posts/create")]
    public async Task<IActionResult> GetPostJdForm()
    {
        var jdSkill = await this.skillService.GetJdGroupSkillS(null);
        var jdLevel = await this.levelService.GetJdLevels(null);
        var jdProvince = await this.provinceService.GetJdProvince(null);
        var domains = await this.domainService.GetDomains();

        this.ViewData["jdSkill"] = jdSkill;
        this.ViewData["jdLevel"] = jdLevel;
        this.ViewData["jdProvince"] = jdProvince;
        this.ViewData["domains"] = domains;

        JdPost? jdPost = null;
        return this.View(ViewMapping.ViewRecruiterCRUJdPost, new RecruitmentJdViewResponseDto(EModeView.Create, jdPost));
    }

    [HttpGet]
    [Route("jd-posts/{jobPostId}")]
    public async Task<IActionResult> GetJobPostDetail(int jobPostId)
    {
        var GetRecruiterIdByJobId = await this.jdPostService.GetRecruiterIdByJobId(jobPostId);

        var currentUserId = GetUserId();
        var currentUserRole = GetUserRole();

        if (currentUserRole == EUserRole.Recruitment.ToString())
        {
            if (currentUserId != GetRecruiterIdByJobId)
            {
                return Redirect("/403");
            }
        }

        var jdSkill = await this.skillService.GetJdGroupSkillS(jobPostId);
        var jdLevel = await this.levelService.GetJdLevels(jobPostId);
        var jdProvince = await this.provinceService.GetJdProvince(jobPostId);
        var domains = await this.domainService.GetDomains();

        this.ViewData["jdSkill"] = jdSkill;
        this.ViewData["jdLevel"] = jdLevel;
        this.ViewData["jdProvince"] = jdProvince;
        this.ViewData["domains"] = domains;
        
        var jdPost = await this.jdPostService.GetJobPostById(jobPostId);
        
        return this.View(ViewMapping.ViewRecruiterCRUJdPost, new RecruitmentJdViewResponseDto(EModeView.Detail, jdPost));
    }

    [HttpGet]
    [Route("job-posts/{jobPostId}/applicants")]
    public async Task<IActionResult> GetViewApplicantsApply(int jobPostId)
    {
        var GetRecruiterIdByJobId = await this.jdPostService.GetRecruiterIdByJobId(jobPostId);

        var currentUserId = GetUserId();
        var currentUserRole = GetUserRole();

        if (currentUserRole == EUserRole.Recruitment.ToString())
        {
            if (currentUserId != GetRecruiterIdByJobId)
            {
                return Redirect("/403");
            }
        }

        var jobPost = await this.jdPostService.GetJobPostById(jobPostId);
        return this.View(ViewMapping.ViewJobPostApplicants, jobPost);
    }
}
