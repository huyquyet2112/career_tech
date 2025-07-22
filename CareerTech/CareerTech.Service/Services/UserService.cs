using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;
using CareerTech.Request.Users;
using CareerTech.Response.Users;
using CareerTech.Service.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace CareerTech.Service.Services;

public class UserService : IUserService
{
    private readonly IUserRepo userRepo;
    private readonly IForgotPasswordRepo forgotPasswordRepo;
    private readonly IEmailService emailService;

    public UserService(IUserRepo userRepo, IForgotPasswordRepo forgotPasswordRepo, IEmailService emailService)
    {
        this.userRepo = userRepo;
        this.forgotPasswordRepo = forgotPasswordRepo;
        this.emailService = emailService;
    }

    public async Task<bool> ChangePassword(ChangePasswordUserDto requestDto, int userId)
    {
        var user = await this.userRepo.FindOneAsync(us => us.Id == userId);

        if (user == default)
        {
            throw new Exception("errUserNotFound");
        }

        user.HashPassword = BC.HashPassword(requestDto.NewPassword);

        await this.userRepo.UpdateOneAsync(user);

        return true;
    }

    public async Task<SendCodeResultDto> GenerateForgotPassword(EmailForgotPasswordDto requestDto)
    {
        try
        {
            var user = await this.userRepo.FindOneAsync(us => us.UserName == requestDto.Email);

            if (user == default)
            {
                return new SendCodeResultDto { Success = false, Message = "errEmailNotExits" };
            }

            var existing = await this.forgotPasswordRepo.FindOneAsync(us => us.Email == requestDto.Email);

            if(existing != default)
            {
                forgotPasswordRepo.Remove(existing);
            }

            var forgotPassword = new ForgotPassword
            {
                Email = requestDto.Email,
                Code = string.Concat(Enumerable.Range(0, 6).Select(_ => new Random().Next(0, 10))),
                ExpiredAt = DateTime.Now.AddMinutes(3)
            };

            await this.forgotPasswordRepo.SaveOneAsync(forgotPassword);

            var message = $"Your verification code is {forgotPassword.Code}. It is valid for 3 minutes.";

            await this.emailService.SendVerificationCode(requestDto.Email, message);

            return new SendCodeResultDto
            {
                Success = true,
                Email = requestDto.Email,
            };

        }
        catch (Exception)
        {
            return new SendCodeResultDto
            {
                Success = false,
                Message = "errGeneric",
            };
        }
    }

    public async Task<SendCodeResultDto> ConfirmVerificationCode(ConfirmVerificationCodeDto requestDto)
    {
        var forgotRecord = await this.forgotPasswordRepo.FindOneAsync(us => us.Email == requestDto.Email);

        if(forgotRecord.ExpiredAt < DateTime.Now)
        {
            forgotPasswordRepo.Remove(forgotRecord);

            return new SendCodeResultDto
            {
                Success = false,
                Message = "errRecordHasExpired"
            };
        }

        if(forgotRecord.Code != requestDto.Code)
        {
            return new SendCodeResultDto
            {
                Success = false,
                Message = "errCodeIsIncorrect"
            };
        }
        else
        {
            return new SendCodeResultDto
            {
                Success = true,
                Email = requestDto.Email
            };
        }
    }

    public async Task<bool> ResetPassword(ResetPasswordDto requestDto)
    {
        var user = await this.userRepo.FindOneAsync(us => us.UserName == requestDto.Email);

        if (user == default)
        {
            throw new Exception("errUserNotFound");
        }

        user.HashPassword = BC.HashPassword(requestDto.NewPassword);

        await this.userRepo.UpdateOneAsync(user);

        return true;
    }
}
