namespace CareerTech.Service.Interfaces;

public interface IEmailService
{
    Task SendVerificationCode(string email, string message);
}
