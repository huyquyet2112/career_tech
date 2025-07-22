using Microsoft.AspNetCore.Mvc;
using CareerTech.Service.Interfaces;
using Microsoft.Extensions.Localization;

namespace CareerTech.Controllers.Api;


[Route("api/job-post-approvals")]
public class ApiJobPostApprovalController : BaseController
{
    private readonly ILogger<ApiJobPostApprovalController> logger;
    private readonly IStringLocalizer<Language> localizer;
    private readonly IJdPostService jobPostService;

    public ApiJobPostApprovalController(
        ILogger<ApiJobPostApprovalController> logger, 
        IStringLocalizer<Language> localizer, 
        IJdPostService jobPostService) : base(localizer)
    {
        this.logger = logger;
        this.localizer = localizer;
        this.jobPostService = jobPostService;
    }

    [HttpGet]
    [Route("{jobPostApprovalId}")]
    public async Task<IActionResult> GetJobPostApprovalById(int jobPostApprovalId)
    {
        var result = await this.jobPostService.GetJobApprovalById(jobPostApprovalId);
        return this.Ok(result);
    }
}
