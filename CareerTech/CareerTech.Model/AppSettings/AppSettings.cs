using CareerTech.Model.Dtos;
using Microsoft.Extensions.Configuration;

namespace CareerTech.Model.AppSettings;

public static class AppSettingProvider
{
    public static AppSettings AppSettings { get; private set; } = new AppSettings();

    public static void Initialize(IConfiguration iConfig)
    {
        AppSettings = new AppSettings();
        var appSettings = iConfig.GetSection("AppSettings");
        appSettings.Bind(AppSettings);
    }
}

public class AppSettings
{
    public JwtModel Jwt { get; set; } = new JwtModel();

    public EmailSettings EmailSettings { get; set; } = new EmailSettings();

    public int MaxFileSize {  get; set; }

    public int MaxFileSizeByte { get; set; }

    public List<LanguageDto> LanguageSupports { get; set; } = [];

    public string[] GetLanguageSupportCodes()
    {
        return this.LanguageSupports.Select(l => l.Code).ToArray();
    }
}

public class JwtModel
{
    public string Secret { get; set; } = string.Empty;

    public int ExpiredTime { get; set; } = 0;
}

public class EmailSettings
{
    public string Sender { get; set; } = string.Empty;

    public string AppPassword {  get; set; } = string.Empty;

    public string Host {  get; set; } = string.Empty;

    public int Port { get; set; } = 0;
}
