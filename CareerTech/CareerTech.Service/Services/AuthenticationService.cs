using CareerTech.Common.Utils;
using CareerTech.Mapper;
using CareerTech.Model.AppSettings;
using CareerTech.Model.Context;
using CareerTech.Model.Dtos;
using CareerTech.Model.Entities;
using CareerTech.Model.Enums;
using CareerTech.Repo.Interfaces;
using CareerTech.Request.Authentication;
using CareerTech.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using BC = BCrypt.Net.BCrypt;

namespace CareerTech.Service.Services;

/// <summary>
/// IAuthenticationService.
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IUserRepo userRepo;
    private readonly IAdminDetailRepo adminDetailRepo;
    private readonly IApplicantDetailRepo applicantDetailRepo;
    private readonly IRecruiterDetailRepo recruitmentDetailRepo;
    private readonly IRoleRepo roleRepo;
    private readonly IUserRoleRepo userRoleRepo;
    private readonly IRolePermissionRepo rolePermissionRepo;
    private readonly IPermissionRepo permissionRepo;
    private readonly DatabaseContext databaseContext;

    public AuthenticationService(
        ILogger<AuthenticationService> logger,
        IUserRepo userRepo,
        IAdminDetailRepo adminDetailRepo,
        IApplicantDetailRepo applicantDetailRepo,
        IRecruiterDetailRepo recruitmentDetailRepo,
        IRoleRepo roleRepo,
        IUserRoleRepo userRoleRepo,
        IRolePermissionRepo rolePermissionRepo,
        IPermissionRepo permissionRepo,
        DatabaseContext databaseContext)
    {
        this._logger = logger;
        this.userRepo = userRepo;
        this.adminDetailRepo = adminDetailRepo;
        this.applicantDetailRepo = applicantDetailRepo;
        this.recruitmentDetailRepo = recruitmentDetailRepo;
        this.roleRepo = roleRepo;
        this.userRoleRepo = userRoleRepo;
        this.permissionRepo = permissionRepo;
        this.rolePermissionRepo = rolePermissionRepo;
        this.databaseContext = databaseContext;
    }

    public UserInfo? ValidateToken(string? token)
    {
        if (token == null)
        {
            return null;
        }

        var jwt = AppSettingProvider.AppSettings.Jwt;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwt.Secret);
        try
        {
            _ = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = false,
                ValidateAudience = false,
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return new UserInfo(jwtToken.Claims);
        }
        catch
        {
            return null;
        }
    }

    public string GenerateJWTAuthentication(UserInfo user, DateTime expiredTime)
    {
        var jwt = AppSettingProvider.AppSettings.Jwt;
        var claims = new List<Claim>
        {
            new (EJwtData.Id.ToString(), user.Id.ToString()),
            new (EJwtData.UserName.ToString(), user.UserName),
            new (EJwtData.Name.ToString(), user.Name),
            new (EJwtData.Role.ToString(), user.Role.ToString()),
            new (EJwtData.VerifyStatus.ToString(), user.VerifyStatus.ToString()),
            new (EJwtData.Avatar.ToString(), user.Avatar.ToString()),
            new (EJwtData.Permissions.ToString(), JsonSerializer.Serialize(Helper.ConvertStringToUnit(user.Permissions.ToList()))),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(claims: claims, expires: expiredTime, signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> Login(LoginDto loginDto)
    {
        var user = await this.userRepo.FindOneAsync(us => us.UserName == loginDto.UserName);

        if (user == default)
        {
            throw new Exception("errUserNotFound");
        }

        if(!BC.Verify(loginDto.Password, user.HashPassword))
        {
            throw new Exception("errPasswordInvalid");
        }

        if(user.Role != loginDto.Role)
        {
            throw new Exception("errRoleMissMatch");
        }

        if(user.Status == EUserStatus.InActive)
        {
            throw new Exception("errUserNoActive");
        }

        string? name = null;
        string? avatar = null;

        if (loginDto.Role == EUserRole.Admin)
        {
            var admin = await adminDetailRepo.FindOneAsync(us => us.UserId == user.Id);
            name = admin.Name;
            avatar = admin.Avatar;
        }
        else if (loginDto.Role == EUserRole.Applicant)
        {
            var applicant = await applicantDetailRepo.FindOneAsync(a => a.UserId == user.Id);
            name = applicant.Name;
            avatar= applicant.Avatar;
        }
        else if (loginDto.Role == EUserRole.Recruitment)
        {
            var recruitment = await recruitmentDetailRepo.FindOneAsync(r => r.UserId == user.Id);
            name = recruitment.Name;
            avatar = recruitment.Avatar;
        }
        else
        {
            throw new Exception("errUnsupportedRole");
        }

        var userPermissions = from userE in this.userRepo.Entities
                              join userRole in this.userRoleRepo.Entities on userE.Id equals userRole.UserId
                              join role in this.roleRepo.Entities on userRole.RoleId equals role.Id
                              join rolePermission in this.rolePermissionRepo.Entities on role.Id equals rolePermission.RoleId
                              join permission in this.permissionRepo.Entities on rolePermission.PermissionId equals permission.Id
                              where userE.Id == user.Id
                              select permission;

        user.Permissions = userPermissions.ToList();

        var userDto = new UserInfo(user, name, avatar);

        var expiredTime = DateTime.Now.AddDays(AppSettingProvider.AppSettings.Jwt.ExpiredTime);
        if (loginDto.StayLogin == default)
        {
            expiredTime = DateTime.Now.AddDays(1);
        }

        return this.GenerateJWTAuthentication(userDto, expiredTime);

    }

    public async Task<bool> Register(RegisterDto registerDto)
    {
        var userNameExit = await this.userRepo.FindOneAsync(us => us.UserName == registerDto.UserName);

        var role = await this.roleRepo.FindOneAsync(us => us.RoleCode == registerDto.Role);

        if(userNameExit != default)
        {
            throw new Exception("errEmailAlreadyExtis");
        }

        var transaction = this.databaseContext.Database.BeginTransaction();

        try
        {
            var user = UserMapper.ToCreateModel(registerDto);
            await this.userRepo.SaveOneAsync(user);

            if(registerDto.Role == EUserRole.Applicant)
            {
                var applicantDetail = new ApplicantDetail
                {
                    UserId = user.Id,
                    Name = registerDto.Name,
                    Email = registerDto.UserName
                };


                await this.applicantDetailRepo.SaveOneAsync(applicantDetail);
            }

            if(registerDto.Role == EUserRole.Recruitment)
            {
                var recruitmentDetail = new RecruiterDetail
                {
                    UserId = user.Id,
                    Name = registerDto.Name,
                    Email = registerDto.UserName,
                };

                await this.recruitmentDetailRepo.SaveOneAsync (recruitmentDetail);
            }

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id,
            };

            await this.userRoleRepo.SaveOneAsync(userRole);

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
