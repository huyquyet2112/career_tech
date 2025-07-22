using CareerTech.Service.Interfaces;
using CareerTech.Response.Apis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using CareerTech.Shared;
using CareerTech.Request.Errors;
using CareerTech.Request.Applicants;
using CareerTech.Request.JobPosts;

namespace CareerTech.Controllers.Api;

[Route("api/applicants")]
public class ApiApplicantController : BaseController
{
    private readonly IStringLocalizer<Language> localizer;
    private readonly IApplicantService applicantService;
    private readonly ILevelService levelService;
    private readonly IProvinceService provinceService;
    private readonly IWebHostEnvironment environment;
    private readonly ISkillService skillService;
    private readonly IJdPostService jdPostService;

    public ApiApplicantController(
        IStringLocalizer<Language> localizer,
        IApplicantService applicantService,
        IWebHostEnvironment environment,
        ISkillService skillService,
        IProvinceService provinceService,
        ILevelService levelService,
        IJdPostService jdPostService) : base(localizer)
    {
        this.localizer = localizer;
        this.applicantService = applicantService;
        this.environment = environment;
        this.skillService = skillService;
        this.provinceService = provinceService;
        this.levelService = levelService;
        this.jdPostService = jdPostService;
    }

    [HttpPut]
    [Route("{userId}/basic-info")]
    public async Task<IActionResult> UpdateApplicantBasicInfor([FromBody] UpdateApplicantBasicInforDto requestDto, int userId)
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

            var updateApplicant = await this.applicantService.UpdateApplicantBasicInfo(requestDto, imagePathFolder, userId);

            if (updateApplicant)
            {
                return this.Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateBasicInforSuccess", this.Localizer)));
            }
            else
            {
                return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateBasicInforFailed", this.Localizer)));
            }
        }
        catch (Exception ex)
        {
            return this.BadRequest(Utils.GetLocalizedMessage($"{ex}", this.localizer));
        }
    }

    [HttpGet]
    [Route("{userId}/skills")]
    public async Task<IActionResult> GetApplicantSkills(int userId)
    {
        var applicantSkills = await this.skillService.GetApplicantGroupSkills(userId);
        return this.Ok(applicantSkills);
    }

    [HttpGet]
    [Route("{userId}/levels")]
    public async Task<IActionResult> GetApplicantLevels(int userId)
    {
        var applicantLevels = await this.levelService.GetApplicantLevels(userId);
        return this.Ok(applicantLevels);
    }

    [HttpGet]
    [Route("{userId}/provinces")]
    public async Task<IActionResult> GetApplicantProvinces(int userId)
    {
        var applicantProvinces = await this.provinceService.GetApplicantProvinces(userId);
        return this.Ok(applicantProvinces);
    }

    [HttpPost]
    [Route("{userId}/skills")]
    public async Task<IActionResult> UpdateApplicantSkills([FromBody] UpdateApplicantSkillDto requestDto, int userId)
    {
        try
        {
            var result = await this.skillService.UpdateApplicantSkill(requestDto);

            if (result)
            {
                return this.Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateSkillSuccess", this.localizer)));
            }

            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateSkillFailed", this.localizer)));
        }
        catch (Exception ex)
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage($"{ex.Message}", this.localizer)));
        }
    }

    [HttpPost]
    [Route("{userId}/levels")]
    public async Task<IActionResult> UpdateApplicantLevels([FromBody] UpdateApplicantLevelDto requestDto, int userId)
    {
        try
        {
            var result = await this.levelService.UpdateApplicantLevel(requestDto);

            if (result)
            {
                return this.Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateLevelSuccess", this.localizer)));
            }

            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateLevelFailed", this.localizer)));
        }
        catch (Exception ex)
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage($"{ex.Message}", this.localizer)));
        }
    }

    [HttpPost]
    [Route("{userId}/provinces")]
    public async Task<IActionResult> UpdateApplicantProvinces([FromBody] UpdateApplicantProvinceDto requestDto, int userId)
    {
        try
        {
            var result = await this.provinceService.UpdateApplicantProvince(requestDto);

            if (result)
            {
                return this.Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateProvinceSuccess", this.localizer)));
            }

            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateProvinceFailed", this.localizer)));
        }
        catch (Exception ex)
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage($"{ex.Message}", this.localizer)));
        }
    }

    [HttpGet]
    [Route("{userId}/cv-files")]
    public async Task<IActionResult> GetCVFilesApplicant(int userId)
    {
        var result = await this.applicantService.ApplicantCvFiles(userId);
        return this.Ok(result);
    }

    [HttpPost]
    [Route("apply-job")]
    public async Task<IActionResult> ApplyJob([FromForm] ApplyJobDto requestDto)
    {
        try
        {
            var result = await this.applicantService.ApplyJob(requestDto);

            if (!result)
            {
                return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgApplyJobFailed", this.localizer)));
            }

            return this.Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgApplyJobSuccess", this.localizer)));
        }
        catch (Exception)
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgApplyJobFailed", this.localizer)));
        }
    }

    [HttpPost]
    [Route("saved-jobs")]
    public async Task<IActionResult> SavedJob([FromBody] UpdateSavedJobDto requestDto)
    {
        try
        {
            var result = await this.jdPostService.UpdateSavedJob(requestDto);

            if (!result)
            {
                return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateSavedJobFailed", this.localizer)));
            }

            return this.Ok();
        }
        catch (Exception)
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUpdateSavedJobFailed", this.localizer)));
        }
    }

    [HttpDelete]
    [Route("saved-jobs/{savedId}")]
    public async Task<IActionResult> DeleteSavedJob(int savedId)
    {
        try
        {
            var result = await this.applicantService.DeleteSavedJob(savedId);

            if (!result)
            {
                return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUnsaveJobPostFailed", this.localizer)));
            }

            return this.Ok();
        }
        catch (Exception)
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgUnsaveJobPostFailed", this.localizer)));
        }
    }
}
