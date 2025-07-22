namespace CareerTech.Response.Apis;

public class UploadFileResponseDto(string path)
{
    public string Path { get; set; } = path;
}
