using CareerTech.Service.Interfaces;
using CareerTech.Model.AppSettings;
using System.Net.Mail;
using System.Net;

namespace CareerTech.Service.Services;

public class EmailService : IEmailService
{
    public async Task SendVerificationCode(string email, string messageBody)
    {
        var emaiSender = AppSettingProvider.AppSettings.EmailSettings.Sender;
        var appPassword = AppSettingProvider.AppSettings.EmailSettings.AppPassword;
        var host = AppSettingProvider.AppSettings.EmailSettings.Host;
        var post = AppSettingProvider.AppSettings.EmailSettings.Port;

        var smtpClient = new SmtpClient(host, post);
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;

        smtpClient.Credentials = new NetworkCredential(emaiSender, appPassword);

        var subject = "Verification Code";
        var message = new MailMessage(emaiSender!, email, subject, messageBody);

        await smtpClient.SendMailAsync(message);

    }
}
