using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using CareerTech.Request.Upload;
using CareerTech.Shared;
using CareerTech.Model.Enums;
using CareerTech.Common.Utils;
using CareerTech.Response.Apis;
using CareerTech.Service.Interfaces;

namespace CareerTech.Controllers.Api;

[Route("api/upload")]
public class ApiUploadController : BaseController
{
    private readonly ILogger<ApiUploadController> logger;
    private readonly IStringLocalizer<Language> localizer;
    private readonly IWebHostEnvironment enviroment;
    private readonly IUploadFileService uploadFileService;

    public ApiUploadController(
        ILogger<ApiUploadController> logger,
        IStringLocalizer<Language> localizer,
        IUploadFileService uploadFileService,
        IWebHostEnvironment enviroment) : base(localizer)
    {
        this.logger = logger;
        this.localizer = localizer;
        this.enviroment = enviroment;
        this.uploadFileService = uploadFileService;
    }

    [HttpPost]
    [Route("avatar")]
    public IActionResult UploadImage([FromForm] FileAvatarUploadDto requestDto)
    {
        if (requestDto.File == null || requestDto.File.Length == 0)
        {
            return this.BadRequest(Utils.GetLocalizedMessage("No File Upload", this.Localizer));
        }

        var urlPath = (string?)null; 

        if (requestDto.TypeFolder == EFolder.RecruiterFolder)
        {
            urlPath = Path.Combine("images", "recruiter-avatar");
        }
        else if(requestDto.TypeFolder == EFolder.ApplicantFolder)
        {
            urlPath = Path.Combine("images", "applicant-avatar");
        }
        else if(requestDto.TypeFolder == EFolder.AdminFolder)
        {
            urlPath = Path.Combine("images", "admin-avatar");
        }

        var uploadFolder = Path.Combine(this.enviroment.WebRootPath, urlPath);

        if(!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var fileName = Helper.GenerateUUID() + Path.GetExtension(requestDto.File.FileName);

        var filePath = Path.Combine(uploadFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            requestDto.File.CopyTo(stream);
        }

        return this.Ok(new UploadFileResponseDto(Path.Combine("/", urlPath, $"{fileName}")));
    }

    [HttpPost]
    [Route("{userId}/fileCV")]
    public async Task<IActionResult> UploadFileCV(int userId, [FromForm] IFormFile fileCV)
    {
        if (fileCV == null || fileCV.Length == 0)
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgNofileSelected", this.Localizer)));
        }

        byte[] fileBytes = new byte[fileCV.Length];

        string fileData;
        using (var stream = fileCV.OpenReadStream())
        {
            await stream.ReadAsync(fileBytes, 0, (int)fileCV.Length);
            fileData = Convert.ToBase64String(fileBytes);
        }

        try
        {
            var cvFile = await this.uploadFileService.AddFile(userId, fileCV.FileName, fileData);
            var messageResponse = new MessageResponseDto(Utils.GetLocalizedMessage("msgUploadFileSuccess", this.Localizer));

            return this.Ok(new
            {
                Message = messageResponse,
                File = cvFile
            });
        }
        catch (Exception ex)
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage($"{ex.Message}", this.localizer)));
        }
    }

    [HttpPut]
    [Route("fileCV/{fileId}")]
    public async Task<IActionResult> DeleteFileCV(int fileId)
    {
        try
        {
            var result = await this.uploadFileService.DeleteFile(fileId);

            if (result)
            {
                return this.Ok(new MessageResponseDto(Utils.GetLocalizedMessage("msgDeleteFileSuccess", this.localizer)));
            }

            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgDeleteFileFailed", this.localizer)));
        }
        catch
        {
            return this.BadRequest(new MessageResponseDto(Utils.GetLocalizedMessage("msgDeleteFileFailed", this.localizer)));
        }
    }
}
