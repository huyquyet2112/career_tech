using CareerTech.Mapper;
using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Model.Enums;
using CareerTech.Repo.Interfaces;
using CareerTech.Request.Admins;
using CareerTech.Request.JobPosts;
using CareerTech.Response.Admins;
using CareerTech.Response.Applicants;
using CareerTech.Response.JobPosts;
using CareerTech.Response.Paginations;
using CareerTech.Response.Recruitments;
using CareerTech.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace CareerTech.Service.Services;

public class AdminService : IAdminService
{
    private readonly ILogger<AdminService> logger;
    private readonly IRecruiterDetailRepo recruiterDetailRepo;
    private readonly IJdPostRepo jdPostRepo;
    private readonly IJdPostApprovalRepo jdPostApprovalRepo;
    private readonly IAdminDetailRepo adminDetailRepo;
    private readonly IApplicantDetailRepo applicantDetailRepo;
    private readonly IJdPostLevelRepo jdPostLevelRepo;
    private readonly IUserRepo userRepo;
    private readonly IDomainRepo domainRepo;
    private readonly IProvincesRepo provinceRepo;
    private readonly ILevelRepo levelRepo;
    private readonly IProvincesJdPostRepo provinceJdPostRepo;
    private readonly IRoleRepo roleRepo;
    private readonly IPermissionRepo permissionRepo;
    private readonly IRolePermissionRepo rolePermissionRepo;
    private readonly DatabaseContext databaseContext;

    public AdminService(
        ILogger<AdminService> logger,
        IRecruiterDetailRepo recruiterDetailRepo,
        IJdPostRepo jdPostRepo,
        IAdminDetailRepo adminDetailRepo,
        IUserRepo userRepo,
        IApplicantDetailRepo applicantDetailRepo,
        ILevelRepo levelRepo,
        IDomainRepo domainRepo,
        IProvincesRepo provinceRepo,
        IProvincesJdPostRepo provinceJdPostRepo,
        IJdPostLevelRepo jdPostLevelRepo,
        IJdPostApprovalRepo jdPostApprovalRepo,
        IRoleRepo roleRepo,
        IPermissionRepo permissionRepo,
        IRolePermissionRepo rolePermissionRepo,
        DatabaseContext databaseContext
        )
    {
        this.logger = logger;
        this.recruiterDetailRepo = recruiterDetailRepo;
        this.jdPostRepo = jdPostRepo;
        this.jdPostApprovalRepo = jdPostApprovalRepo;
        this.adminDetailRepo = adminDetailRepo;
        this.applicantDetailRepo = applicantDetailRepo;
        this.domainRepo = domainRepo;
        this.provinceRepo = provinceRepo;
        this.levelRepo = levelRepo;
        this.provinceJdPostRepo = provinceJdPostRepo;
        this.jdPostLevelRepo = jdPostLevelRepo;
        this.userRepo = userRepo;
        this.roleRepo = roleRepo;
        this.permissionRepo = permissionRepo;
        this.rolePermissionRepo = rolePermissionRepo;
        this.databaseContext = databaseContext;
    }


    public async Task<IList<JobPostAdminResponseDto>> GetJobPostsForAdmin()
    {
        var jobPosts = await this.jdPostRepo.Entities.OrderByDescending(x => x.CreatedAt).ToListAsync();

        var jobPostDtos = new List<JobPostAdminResponseDto>();

        foreach (var jobPost in jobPosts)
        {
            var statusApprovalJob = (await this.jdPostApprovalRepo.FindManyAsync(us => us.JdPostId == jobPost.Id)).OrderByDescending(x => x.Id).FirstOrDefault();
            var recruitmentDetail = await this.recruiterDetailRepo.FindOneAsync(us => us.UserId == jobPost.UserId);

            var jobPostDto = new JobPostAdminResponseDto
            {
                JobPostId = jobPost.Id,
                Title = jobPost.Title,
                Status = jobPost.Status,
                StatusApproval = statusApprovalJob != null ? statusApprovalJob.Status : null,
                RecruiterId = recruitmentDetail.Id,
                NameRecruiter = recruitmentDetail.Name,
                UserId = recruitmentDetail.UserId
            };

            jobPostDtos.Add(jobPostDto);
        }

        return jobPostDtos;
    }

    public async Task<bool> CreateJobPostApproval(AddJobPostApprovalDto requestDto)
    {
        try
        {
            var jobPostApproval = JobPostApprovalMapper.ToCreateModel(requestDto);

            await this.jdPostApprovalRepo.SaveOneAsync(jobPostApproval);

            return true;
        }
        catch
        {
            throw;
        }

    }

    public async Task<AdminDetail> Detail(int userId)
    {
        return await this.adminDetailRepo.FindOneAsync(us => us.UserId == userId);
    }

    public async Task<bool> UpdateAdminDetail(UpdateAdminDetailDto requestDto, string imagePathFolder, int userId)
    {
        var user = await this.userRepo.FindOneAsync(us => us.Id == userId);

        if (user == default)
        {
            throw new Exception("errUserNotFound");
        }

        var admin = await this.adminDetailRepo.FindOneAsync(us => us.UserId == userId);

        if (admin == default)
        {
            throw new Exception("errAdminInfoNotFound");
        }

        var transaction = this.databaseContext.Database.BeginTransaction();

        try
        {
            var adminUpdate = AdminMapper.ToUpdateModel(admin, requestDto);
            await this.adminDetailRepo.UpdateOneAsync(adminUpdate);
            await transaction.CommitAsync();

            if (adminUpdate.Avatar != requestDto.OldAvatar)
            {
                var oldImagePath = Path.Combine(imagePathFolder, $"{requestDto.OldAvatar}".TrimStart('/').Replace("\\", "\\"));

                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath);
                }
            }

            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            transaction.Dispose();
        }
    }

    public async Task<IList<RecruiterPublicDto>> GetAdminRecruiters(QueryUserDto query)
    {
        var recruiters = await (from r in this.recruiterDetailRepo.Entities
                                join u in this.userRepo.Entities on r.UserId equals u.Id
                                where (query.Status == default || query.Status.Contains(u.Status))
                                      && (string.IsNullOrEmpty(query.Name) || r.Name.ToLower().Contains(query.Name.ToLower()))
                                select r).ToListAsync();

        var recruiterPublicsDto = new List<RecruiterPublicDto>();

        var jobCountByRecruiter = await (
            from p in jdPostRepo.Entities
            join a in jdPostApprovalRepo.Entities on p.Id equals a.JdPostId
            where a.Id == jdPostApprovalRepo.Entities
            .Where(a2 => a2.JdPostId == p.Id)
            .Max(a2 => a2.Id)
            group p by p.UserId into g
            select new
            {
                RecruiterId = g.Key,
                JobCount = g.Count()
            }).ToDictionaryAsync(x => x.RecruiterId, x => x.JobCount);

        foreach (var recruiter in recruiters)
        {
            var user = await this.userRepo.FindOneAsync(us => us.Id == recruiter.UserId);
            var recruiterDto = new RecruiterPublicDto
            {
                RecruiterId = recruiter.Id,
                JobPostPuclic = jobCountByRecruiter.TryGetValue(recruiter.UserId, out var count) ? count : 0,
                RecruiterName = recruiter.Name,
                RecuiterAvatar = recruiter.Avatar,
                CompanySize = recruiter.CompanySize,
                WorkSchedule = recruiter.WorkSchedule,
                UserId = recruiter.UserId,
                Status = user.Status,
                VerifyStatus = user.VerifyStatus,
            };

            recruiterPublicsDto.Add(recruiterDto);
        }

        return recruiterPublicsDto;
    }

    public async Task<IList<ApplicantAdminDto>> GetAdminApplicants(QueryUserDto query)
    {
        var applicants = await (from a in this.applicantDetailRepo.Entities
                                join u in this.userRepo.Entities on a.UserId equals u.Id
                                where (query.Status == default || query.Status.Contains(u.Status))
                                      && (string.IsNullOrEmpty(query.Name) || a.Name.ToLower().Contains(query.Name.ToLower()))
                                select a).ToListAsync();

        var applicantsDto = new List<ApplicantAdminDto>();

        foreach (var applicant in applicants)
        {
            var user = await this.userRepo.FindOneAsync(us => us.Id == applicant.UserId);

            var applicantDto = new ApplicantAdminDto
            {
                Id = applicant.Id,
                UserId = user.Id,
                Status = user.Status,
                Name = applicant.Name,
                Avatar = applicant.Avatar,
                Email = applicant.Email
            };

            applicantsDto.Add(applicantDto);
        }

        return applicantsDto;
    }

    public async Task<StatusUserDto> GetStatusUser(int userId)
    {
        var user = await this.userRepo.FindOneAsync(us => us.Id == userId);

        return new StatusUserDto { Status = user.Status, VerifyStatus = user.VerifyStatus };
    }

    public async Task<bool> UpdateStatusUser(UpdateStatusUserDto requestDto, int userId)
    {
        try
        {
            var user = await this.userRepo.FindOneAsync(us => us.Id == userId);

            user.Status = requestDto.Status;
            user.VerifyStatus = requestDto.VerifyStatus;

            await this.userRepo.UpdateOneAsync(user);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<ViewJobPostPaginationDto> GetJobPostByRecruiterForAdmin(QueryListJobPostDto query, int userId)
    {
        var jobPosts = await (from p in this.jdPostRepo.Entities
                              join a in this.jdPostApprovalRepo.Entities on p.Id equals a.JdPostId
                              join jl in this.jdPostLevelRepo.Entities on p.Id equals jl.JdPostId
                              join pj in this.provinceJdPostRepo.Entities on p.Id equals pj.JdPostId
                              where a.Id == this.jdPostApprovalRepo.Entities
                                  .Where(a2 => a2.JdPostId == p.Id)
                                  .Max(a2 => a2.Id)
                              where p.UserId == userId
                              where (query.Levels == default || query.Levels.Contains(jl.LevelId))
                                    && (query.Provinces == default || query.Provinces.Contains(pj.ProvinceId))
                                    && (string.IsNullOrEmpty(query.Title) || p.Title.ToLower().Contains(query.Title.ToLower()))
                              select p).Distinct().ToListAsync();

        var totalRecord = jobPosts.Count();

        var jobPostDtos = new List<JobPostResponseDto>();

        foreach (var job in jobPosts)
        {
            var statusApprovalJob = (await this.jdPostApprovalRepo.FindManyAsync(us => us.JdPostId == job.Id)).OrderByDescending(x => x.Id).FirstOrDefault();
            var jobPostLevels = await this.jdPostLevelRepo.FindManyAsync(us => us.JdPostId == job.Id);
            var jobPostProvinces = await this.provinceJdPostRepo.FindManyAsync(us => us.JdPostId == job.Id);
            var user = await this.recruiterDetailRepo.FindOneAsync(us => us.UserId == job.UserId);
            var domain = await this.domainRepo.FindOneAsync(us => us.Id == job.DomainId);

            var jobPostDto = new JobPostResponseDto
            {
                Id = job.Id,
                RecruiterId = user.UserId,
                NameRecruiter = user.Name,
                AvatarRecruiter = user.Avatar,
                Title = job.Title,
                MinSalary = job.MinSalary,
                MaxSalary = job.MaxSalary,
                CurrencySalary = job.CurrencySalary,
                UpdatedAt = job.UpdatedAt,
                ExperienceYear = job.ExperienceYear,
                Domain = domain.Name,
                WorkingType = job.WorkingType,
                Status = job.Status,
                StatusApproval = statusApprovalJob != null ? statusApprovalJob.Status : null,
            };

            var jobLevelDto = new List<Level>();
            var jobProvinceDto = new List<Province>();

            foreach (var jobPostLevel in jobPostLevels)
            {
                var level = await this.levelRepo.FindOneAsync(us => us.Id == jobPostLevel.LevelId);

                jobLevelDto.Add(level);
            }

            jobPostDto.Level = jobLevelDto;


            foreach (var jobPostProvince in jobPostProvinces)
            {
                var province = await this.provinceRepo.FindOneAsync(us => us.Id == jobPostProvince.ProvinceId);

                jobProvinceDto.Add(province);
            }

            jobPostDto.Province = jobProvinceDto;

            jobPostDtos.Add(jobPostDto);
        }

        var jobs = jobPostDtos.Skip(query.GetSkip()).Take(query.PageSize);

        var pagination = new PaginationDto(query.Page, query.PageSize, totalRecord);

        return new ViewJobPostPaginationDto(jobs, pagination);
    }

    public async Task<IList<UserRoleStatisticDto>> GetUserRoleStatisticDtos()
    {
        var users = await this.userRepo.GetAllAsync();

        var result = users.GroupBy(us => us.Role).Select(ut => new UserRoleStatisticDto
        {
            Role = ut.Key,
            Count = ut.Count()
        }).ToList();

        return result;
    }

    public async Task<IList<JobStatusStatisticDto>> GetJobStatusStatisticDtos()
    {
        var approvals = await (
            from a in this.jdPostApprovalRepo.Entities
            where a.Id == this.jdPostApprovalRepo.Entities
                .Where(x => x.JdPostId == a.JdPostId)
                .Max(x => x.Id)
            group a by a.Status into g
            select new JobStatusStatisticDto
            {
                StatusApproval = g.Key,
                Count = g.Count()
            }).ToListAsync();

        var allStatuses = Enum.GetValues(typeof(EJdPostApproval)).Cast<EJdPostApproval>();

        var fullResult = allStatuses
            .Select(status => new JobStatusStatisticDto
            {
                StatusApproval = status,
                Count = approvals.FirstOrDefault(x => x.StatusApproval == status)?.Count ?? 0
            }).ToList();

        return fullResult;

    }

    public async Task<IList<Role>> GetRoles()
    {
        var roles = await this.roleRepo.GetAllAsync();

        return roles;
    }

    public async Task<Role> RoleDetail(int roleId)
    {
        var role = await this.roleRepo.FindOneAsync(us => us.Id == roleId);
        return role;
    }

    public async Task<IList<RolePermissionResponseDto>> GetRolePermissions(int roleId)
    {
        var permissions = await this.permissionRepo.GetAllAsync();

        var rolePermissions = await this.rolePermissionRepo.FindManyAsync(us => us.RoleId == roleId);

        var result = new List<RolePermissionResponseDto>();

        var selected = false;

        foreach(var permission in permissions)
        {
            selected = rolePermissions.Any(us => us.PermissionId == permission.Id);
            result.Add(new RolePermissionResponseDto(permission, selected));
        }

        return result;
    }

    public async Task<IList<RolePermission>> UpdateRolePermissions(UpdateRolePermissionDto requestDto)
    {
        
        var transaction = this.databaseContext.Database.BeginTransaction();

        try
        {
            if(requestDto.Permissions.Count == 0)
            {
                var removePermissions = await this.rolePermissionRepo.FindManyAsync(us => us.RoleId == requestDto.RoleId);
                this.rolePermissionRepo.Remove(removePermissions);
            }
            else
            {
                var permissionsRequest = await this.permissionRepo.FindManyAsync(us => requestDto.GetPermissionIds().Contains(us.Id));
                var olderPermissions = await this.rolePermissionRepo.FindManyAsync(us => us.RoleId == requestDto.RoleId);
                var removePermissions = olderPermissions.ExceptBy(permissionsRequest.Select(us => us.Id), ut => ut.PermissionId).ToList();
                var addPermissions = permissionsRequest.ExceptBy(olderPermissions.Select(us => us.PermissionId), ut => ut.Id).ToList();

                if(removePermissions.Count != 0)
                {
                    this.rolePermissionRepo.Remove(removePermissions);
                }

                if(addPermissions.Count != 0)
                {
                    var rolePermissions = addPermissions.Select(permission => new RolePermission
                    {
                        PermissionId = permission.Id,
                        RoleId = requestDto.RoleId,
                    });

                    await this.rolePermissionRepo.SaveManyAsync(rolePermissions);
                }
            }

            await transaction.CommitAsync();
            return await this.rolePermissionRepo.FindManyAsync(us => us.RoleId == requestDto.RoleId);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
