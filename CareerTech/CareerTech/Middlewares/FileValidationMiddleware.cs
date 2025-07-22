using Microsoft.Extensions.Localization;
using CareerTech.Response.Apis;
using CareerTech.Shared;

namespace CareerTech.Middlewares;

public class FileValidationMiddleware(
    RequestDelegate next,
    IStringLocalizer<Language> localizer
    )
{
    private readonly RequestDelegate next = next;
    private readonly IStringLocalizer localizer = localizer;

    public async Task InvokeAsync(HttpContext context)
    {
        if ((context.Request.Path.StartsWithSegments("/api/upload")) && context.Request.Method == HttpMethods.Post)
        {
            if (!context.Request.HasFormContentType || !context.Request.Form.Files.Any())
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new MessageResponseDto(Utils.GetLocalizedMessage("errNoFileUpload", this.localizer)));
                return;
            }

            var files = context.Request.Form.Files;
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".jfif", ".pdf", ".docx", ".doc" };
            foreach (var file in files)
            {
                if (file.Length > AppSettingProvider.AppSettings.MaxFileSize * 1024 * 1024)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new MessageResponseDto(Utils.GetLocalizedMessage("errFileTooLarge", this.localizer)));
                    return;
                }

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new MessageResponseDto(Utils.GetLocalizedMessage("errFileTypeNotAllowed", this.localizer)));
                    return;
                }

                if (!this.IsValidFileSignature(file))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new MessageResponseDto(Utils.GetLocalizedMessage("errInvalidFileContent", this.localizer)));
                    return;
                }
            }
        }

        await this.next(context);
    }

    public Dictionary<string, byte[]> FileSingatures = new()
    {
        { ".jpg", new byte[] { 0xFF, 0xD8, 0xFF } },
        { ".jpeg", new byte[] { 0xFF, 0xD8, 0xFF } },
        { ".png", new byte[] { 0x89, 0x50, 0x4E, 0x47 } },
        { ".jfif", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 } },
        { ".pdf", new byte[] { 0x25, 0x50, 0x44, 0x46 } },
        { ".doc", new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 } },
        { ".docx", new byte[] { 0x50, 0x4B, 0x03, 0x04 } },
    };

    /// <summary>
    /// IsValidFileSignature.
    /// </summary>
    /// <param name="file">file.</param>
    /// <returns>true of false.</returns>
    public bool IsValidFileSignature(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!this.FileSingatures.ContainsKey(extension))
        {
            return false;
        }

        using var stream = file.OpenReadStream();
        var headerBytes = new byte[this.FileSingatures[extension].Length];
        stream.Read(headerBytes, 0, headerBytes.Length);

        return headerBytes.SequenceEqual(this.FileSingatures[extension]);
    }
}
