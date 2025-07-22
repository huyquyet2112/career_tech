using CareerTech.Attributes;
using CareerTech.Repo.Interfaces;
using CareerTech.Repo.Repositories;
using CareerTech.Service.Interfaces;
using CareerTech.Service.Services;
using Microsoft.AspNetCore.Authorization;

namespace CareerTech.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepo, UserRepo>();
        services.AddScoped<IRoleRepo, RoleRepo>();
        services.AddScoped<IPermissionRepo, PermissionRepo>();
        services.AddScoped<IUserRoleRepo, UserRoleRepo>();
        services.AddScoped<IRolePermissionRepo, RolePermissionRepo>();
        services.AddScoped<IAdminDetailRepo, AdminDetailRepo>();
        services.AddScoped<IRecruiterDetailRepo, RecruiterDetailRepo>();
        services.AddScoped<IApplicantDetailRepo, ApplicantDetailRepo>();
        services.AddScoped<ISkillRepo, SkillRepo>();
        services.AddScoped<IJdPostRepo, JdPostRepo>();
        services.AddScoped<ICvFileRepo, CvFileRepo>();
        services.AddScoped<IApplyJobRepo, ApplyJobRepo>();
        services.AddScoped<IGroupDomainRepo, GroupDomainRepo>();
        services.AddScoped<IDomainRepo, DomainRepo>();
        services.AddScoped<ILevelRepo, LevelRepo>();
        services.AddScoped<IProvincesRepo, ProvincesRepo>();
        services.AddScoped<IProvincesJdPostRepo, ProvincesJdPostRepo>();
        services.AddScoped<IJdPostLevelRepo, JdPostLevelRepo>();
        services.AddScoped<IGroupSkillRepo, GroupSkillRepo>();
        services.AddScoped<ICertificateRepo, CertificateRepo>();
        services.AddScoped<IApplicantSkillRepo, ApplicantSkillRepo>();
        services.AddScoped<IApplicantCertificateRepo, ApplicantCertificateRepo>();
        services.AddScoped<IJdPostSkillRepo, JdPostSkillRepo>();
        services.AddScoped<IJdPostCertificateRepo, JdPostCertificateRepo>();
        services.AddScoped<IRecruiterDomainRepo, RecruiterDomainRepo>();
        services.AddScoped<IJdPostApprovalRepo, JdPostApprovalRepo>();
        services.AddScoped<ISavedJdPostRepo, SavedJdPostRepo>();
        services.AddScoped<IForgotPasswordRepo, ForgotPasswordRepo>();
        services.AddScoped<IApplicantLevelRepo, ApplicantLevelRepo>();
        services.AddScoped<IApplicantProvinceRepo, ApplicantProvinceRepo>();

        return services;
    }

    public static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IRecruiterService, RecruiterService>();
        services.AddTransient<IDomainService, DomainService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ISkillService, SkillService>();
        services.AddTransient<ILevelService, LevelService>();
        services.AddTransient<IProvinceService, ProvinceService>();
        services.AddTransient<IJdPostService, JdPostService>();
        services.AddTransient<IAdminService, AdminService>();
        services.AddTransient<IApplicantService, ApplicantService>();
        services.AddTransient<IUploadFileService, UploadFileService>();
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }

    public static IServiceCollection AddHandler(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionRequireHandler>();

        return services;
    }
}
