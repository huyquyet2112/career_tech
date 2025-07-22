using CareerTech.Model.Enums;
using Microsoft.AspNetCore.Http;


namespace CareerTech.Request.Upload;

/// <summary>
/// FileAvatarUploadDto.
/// </summary>
public class FileAvatarUploadDto
{
    /// <summary>
    /// Gets or sets File.
    /// </summary>
    public IFormFile? File { get; set; }

    /// <summary>
    /// Gets pr sets TypeFolder.
    /// </summary>
    public EFolder TypeFolder { get; set; }
}
