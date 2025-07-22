using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using CareerTech.Shared;
using Microsoft.AspNetCore.Authorization;
using CareerTech.Service.Interfaces;
using CareerTech.Request.JobPosts;
using CareerTech.Response.Applicants;
using CareerTech.Request.Recruitments;
using CareerTech.Model.Enums;

namespace CareerTech.Controllers;

[Route("applicants")]
public class ApplicantController : BaseController
{
    private readonly ILogger<ApplicantController> logger;
    private readonly IStringLocalizer<Language> localizer;
    private readonly IApplicantService applicantService;
    private readonly ISkillService skillService;
    private readonly IJdPostService jobPostService;
    private readonly IProvinceService provinceService;
    private readonly IDomainService domainService;
    private readonly ILevelService levelService;
    private readonly IRecruiterService recruiterService;

    public ApplicantController(
        ILogger<ApplicantController> logger,
        IStringLocalizer<Language> localizer,
        IApplicantService applicantService,
        ISkillService skillService,
        IJdPostService jobPostService,
        IProvinceService provinceService,
        IDomainService domainService,
        ILevelService levelService,
        IRecruiterService recruiterService) : base(localizer)
    {
        this.localizer = localizer;
        this.logger = logger;
        this.applicantService = applicantService;
        this.skillService = skillService;
        this.jobPostService = jobPostService;
        this.recruiterService = recruiterService;
        this.levelService = levelService;
        this.domainService = domainService;
        this.jobPostService = jobPostService;
        this.provinceService = provinceService;
    }


    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var recruiters = await this.recruiterService.GetRecruitersIndex();
        var jobPosts = await this.jobPostService.GetJobPostPulicIndex();

        this.ViewData["jobPosts"] = jobPosts;
        this.ViewData["recruiters"] = recruiters;
        return this.View(ViewMapping.ViewApplicantIndex);
    }

    [HttpGet]
    [Route("{userId}/profile")]
    public async Task<IActionResult> Detail(int userId)
    {
        var currentUserId = GetUserId();
        var currentUserRole = GetUserRole();

        if (currentUserRole == EUserRole.Applicant.ToString())
        {
            if(currentUserId != userId)
            {
                return Redirect("/403");
            }
        }

        var applicant = await this.applicantService.Detail(userId);
        var applicantSkills = await this.skillService.GetApplicantGroupSkills(userId);
        var applicantCvFiles = await this.applicantService.ApplicantCvFiles(userId);
        var applicantLevels = await this.levelService.GetApplicantLevels(userId);
        var applicantProvinces = await this.provinceService.GetApplicantProvinces(userId);

        this.ViewData["applicantSkills"] = applicantSkills;
        this.ViewData["applicantCvFiles"] = applicantCvFiles;
        this.ViewData["applicantLevels"] = applicantLevels;
        this.ViewData["applicantProvinces"] = applicantProvinces;

        return this.View(ViewMapping.ViewApplicantDetail, applicant);
    }

    [HttpGet]
    [Route("{userId}/saved-jobs")]
    public async Task<IActionResult> RenderViewSavedJobs(QueryApplicationJobPost query, int userId)
    {
        var currentUserId = GetUserId();
        var currentUserRole = GetUserRole();

        if (currentUserRole == EUserRole.Applicant.ToString())
        {
            if (currentUserId != userId)
            {
                return Redirect("/403");
            }
        }
        var savedJobPosts = await this.applicantService.GetSavedJobPost(query, userId);
        return this.View(ViewMapping.ViewApplicantSavedJob, savedJobPosts);
    }

    [HttpGet]
    [Route("{userId}/applied-jobs")]
    public async Task<IActionResult> RenderViewAppliedJobs(QueryApplicationJobPost query, int userId)
    {
        var currentUserId = GetUserId();
        var currentUserRole = GetUserRole();

        if (currentUserRole == EUserRole.Applicant.ToString())
        {
            if (currentUserId != userId)
            {
                return Redirect("/403");
            }
        }
        var appliedJobPosts = await this.applicantService.GetAppliedJobPosts(query, userId);
        return this.View(ViewMapping.ViewApplicantAppliedJob, appliedJobPosts);
    }

    [HttpGet]
    [Route("jobs")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicJobs(QueryListJobPostDto query)
    {
        var jobs = await this.jobPostService.GetJobPostPublic(query);
        var levels = await this.levelService.GetLevels();
        var provinces = await this.provinceService.GetProvinces();
        var skills = await this.skillService.GetSkills();

        this.ViewData["query"] = query;
        this.ViewData["levels"] = levels;
        this.ViewData["provinces"] = provinces;
        this.ViewData["skills"] = skills;

        return this.View(ViewMapping.ViewJobsPublic, jobs);

    }

    [HttpGet]
    [Route("jobs/{jobPostId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDetailJobPublic(int jobPostId)
    {
        var userId = GetUserId();

        var jobPost = await this.jobPostService.GetDetailJobPost(jobPostId, userId);
        var recruitmentDetail = await this.recruiterService.GetRecruiterDetail(jobPost.RecruiterId);
        var recruitmentsDomain = await this.recruiterService.GetGroupDomainRecruiter(jobPost.RecruiterId);
        var jdSkill = await this.skillService.GetJdGroupSkillS(jobPostId);
        var jdLevel = await this.levelService.GetJdLevels(jobPostId);
        var jdProvince = await this.provinceService.GetJdProvince(jobPostId);
        var domains = await this.domainService.GetDomains();

        this.ViewData["jdSkill"] = jdSkill;
        this.ViewData["jdLevel"] = jdLevel;
        this.ViewData["jdProvince"] = jdProvince;
        this.ViewData["domains"] = domains;
        this.ViewData["recruitmentDetail"] = recruitmentDetail;
        this.ViewData["recruitmentDomains"] = recruitmentsDomain;


        return this.View(ViewMapping.ViewPublicDetailJobPost, jobPost);
    }

    [HttpGet]
    [Route("public-recruiters")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicRecruiter(QueryRecruiterPublicDto query)
    {
        var recruiters = await this.recruiterService.GetRecruiters(query);
        var groupDomains = await this.domainService.GetAllGroupDomain();

        this.ViewData["groupDomains"] = groupDomains;
        this.ViewData["query"] = query;

        return this.View(ViewMapping.ViewRecruitersPublic, recruiters);
    }

    [HttpGet]
    [Route("public-recruiters/{userId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProfileRecruiterPublic(QueryListJobPostDto query, int userId)
    {
        var recruiter = await this.recruiterService.GetRecruiterDetail(userId);
        var recruiterDomains = await this.recruiterService.GetGroupDomainRecruiter(userId);
        var jobPostPublicByRecruiter = await this.recruiterService.GetJobPostPublicByRecruiter(query, userId);
        var levels = await this.levelService.GetLevels();
        var provinces = await this.provinceService.GetProvinces();

        this.ViewData["levels"] = levels;
        this.ViewData["provinces"] = provinces;
        this.ViewData["recruiterDomains"] = recruiterDomains;
        this.ViewData["recruiterDetail"] = recruiter;
        this.ViewData["query"] = query;
        return this.View(ViewMapping.ViewRecruiterProfilePublic, jobPostPublicByRecruiter);
    }
}
