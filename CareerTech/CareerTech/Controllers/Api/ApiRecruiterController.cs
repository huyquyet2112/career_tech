using CareerTech.Request.Recruitments;
using CareerTech.Service.Interfaces;
using CareerTech.Response.Apis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using CareerTech.Shared;
using CareerTech.Request.Errors;
using CareerTech.Request.JobPosts;
using CareerTech.Request.Applicants;

namespace CareerTech.Controllers.Api;

[Route("api/recruiters")]
public class ApiRecruiterController : BaseController
{
    private readonly ILogger<ApiRecruiterController> logger;
    private readonly IStringLocalizer<Language> localizer;
    private readonly IRecruiterService recruiterService;
    private readonly IJdPostService jdPostService;
    private readonly IWebHostEnvironment environment;

    public ApiRecruiterController(
        ILogger<ApiRecruiterController> logger,
        IStringLocalizer<Language> localizer,
        IRecruiterService recruiterService,
        IJdPostService jdPostService,
        IWebHostEnvironment environment) : base(localizer)
    {
        this.logger = logger;
        this.localizer = localizer;
        this.recruiterService = recruiterService;
        this.jdPostService = jdPostService;
        this.environment = environment;
    }

    [HttpPut]
    [Route("group-domain")]
    public async Task<IActionResult> UpdateRecruiterGroupDomain([FromBody] UpdateRecruiterGroupDomainDto requestDto)
    {
        try
        {
            var result = await this.recruiterService.UpdateRecruiterGroupDomain(requestDto);

            return this.Ok(new
            {
                Message = new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateGroupDomainSuccess", this.localizer)),
                RecruitmentGroupDomains = result,
            });
        }
        catch (Exception ex)
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage($"{ex.Message}", this.localizer)));
        }
    }

    [HttpPut]
    [Route("{userId}/basic-info")]
    public async Task<IActionResult> UpdateBasicInfo([FromBody] UpdateRecruiterBasicInfoDto requestDto, int userId)
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

            var updateRecruitment = await this.recruiterService.UpdateRecruiterBasicInfo(requestDto, imagePathFolder, userId);

            if (updateRecruitment)
            {
                return this.Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateRecruitmentSuccess", this.Localizer)));
            }
            else
            {
                return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateRecruitmentFailed", this.Localizer)));
            }
        }
        catch (Exception ex)
        {
            return this.BadRequest(Utils.GetLocalizedMessage($"{ex}", this.localizer));
        }
    }

    [HttpGet]
    [Route("{userId}/job-posts")]
    public async Task<IActionResult> GetRecruiterJobPostList(int userId)
    {
        var result = await this.recruiterService.GetRecruiterJobPostsAsync(userId);
        return this.Ok(result);
    }

    [HttpPost]
    [Route("job-posts")]
    public async Task<IActionResult> CreateJobPost([FromBody] AddRecruiterJdDto requestDto)
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

            var result = await this.recruiterService.CreateJobPost(requestDto);

            if (!result)
            {
                return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgCreateJdFailed", this.localizer)));
            }

            return this.Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgCreateJdSuccess", this.localizer)));
        }
        catch
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgCreateJdFailed", this.localizer)));
        }
    }

    [HttpPut]
    [Route("job-posts/{jobPostId}")]
    public async Task<IActionResult> UpdateJobPost([FromBody] UpdateRecruiterJdDto requestDto)
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

            var result = await this.recruiterService.UpdateJobPost(requestDto);

            if (!result)
            {
                return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateJdFailed", this.localizer)));
            }

            return this.Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateJdSuccess", this.localizer)));
        }
        catch
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateJdFailed", this.localizer)));
        }
    }

    [HttpGet]
    [Route("job-posts/{jobPostId}/applicants")]
    public async Task<IActionResult> GetJobPostApplicants(int jobPostId)
    {
        var result = await this.jdPostService.GetJobPostApplicants(jobPostId);
        return this.Ok(result);
    }

    [HttpGet]
    [Route("applications/{applyId}")]
    public async Task<IActionResult> GetJobPostApplication(int applyId)
    {
        var result = await this.jdPostService.GetDetailApplicantion(applyId);

        return this.Ok(result);
    }

    [HttpPut]
    [Route("job-posts/applications/{applyId}/status")]
    public async Task<IActionResult> UpdateApplyJob(int applyId)
    {
        var result = await this.jdPostService.UpdateStatusApplication(applyId);

        if (result)
        {
            return this.Ok();
        }
        else
        {
            return this.BadRequest();
        }
    }

    [HttpPut]
    [Route("job-posts/applications/{applyId}/fit-status")]
    public async Task<IActionResult> UpdateFitStatus([FromBody] JobPostAppicantionFitStatusDto requestDto, int applyId)
    {
        try
        {
            var result = await this.jdPostService.UpdateFitStatusApplication(requestDto, applyId);

            if (result)
            {
                return this.Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateFitStatusSuccess", this.localizer)));
            }

            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateFitStatusFailed", this.localizer)));
        }
        catch (Exception)
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateFitStatusFailed", this.localizer)));
        }
    }

    [HttpGet]
    [Route("public-applicants")]
    public async Task<IActionResult> GetPublicApplicants(QueryApplicantPublicDto query)
    {
        var result = await this.recruiterService.GetApplicantsPublic(query);
        return this.Ok(result);
    }
}
