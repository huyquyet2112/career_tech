using CareerTech.Mapper;
using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Model.Enums;
using CareerTech.Repo.Interfaces;
using CareerTech.Request.Applicants;
using CareerTech.Request.JobPosts;
using CareerTech.Request.Recruitments;
using CareerTech.Response.Admins;
using CareerTech.Response.Applicants;
using CareerTech.Response.JobPosts;
using CareerTech.Response.Paginations;
using CareerTech.Response.Recruitments;
using CareerTech.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CareerTech.Service.Services;

public class RecruiterService : IRecruiterService
{
    private readonly ILogger<RecruiterService> logger;
    private readonly IRecruiterDetailRepo recruiterDetailRepo;
    private readonly IUserRepo userRepo;
    private readonly IDomainService domainService;
    private readonly IRecruiterDomainRepo recruiterDomainRepo;
    private readonly IGroupDomainRepo groupDomainRepo;
    private readonly IJdPostRepo jdPostRepo;
    private readonly IJdPostApprovalRepo jdPostApprovalRepo;
    private readonly IJdPostSkillRepo jdPostSkillRepo;
    private readonly IJdPostLevelRepo jdPostLevelRepo;
    private readonly IProvincesJdPostRepo provinceJdPostRepo;
    private readonly ILevelRepo levelRepo;
    private readonly ISkillRepo skillRepo;
    private readonly IProvincesRepo provinceRepo;
    private readonly IDomainRepo domainRepo;
    private readonly IApplicantDetailRepo applicantDetailRepo;
    private readonly IApplicantLevelRepo applicantLevelRepo;
    private readonly IApplicantSkillRepo applicantSkillRepo;
    private readonly IApplicantProvinceRepo applicantProvinceRepo;
    private readonly DatabaseContext databaseContext;

    public RecruiterService(
        ILogger<RecruiterService> logger,
        IRecruiterDetailRepo recruiterDetailRepo,
        IUserRepo userRepo,
        IDomainService domainService,
        IRecruiterDomainRepo recruiterDomainRepo,
        IGroupDomainRepo groupDomainRepo,
        IJdPostRepo jdPostRepo,
        IJdPostApprovalRepo jdPostApprovalRepo,
        IJdPostSkillRepo jdPostSkillRepo,
        IJdPostLevelRepo jdPostLevelRepo,
        IProvincesJdPostRepo provinceJdPostRepo,
        ILevelRepo levelRepo,
        ISkillRepo skillRepo,
        IProvincesRepo provinceRepo,
        IDomainRepo domainRepo,
        IApplicantDetailRepo applicantDetailRepo,
        IApplicantLevelRepo applicantLevelRepo,
        IApplicantSkillRepo applicantSkillRepo,
        IApplicantProvinceRepo applicantProvinceRepo,
        DatabaseContext databaseContext)
    {
        this.logger = logger;
        this.recruiterDetailRepo = recruiterDetailRepo;
        this.userRepo = userRepo;
        this.domainService = domainService;
        this.recruiterDomainRepo = recruiterDomainRepo;
        this.groupDomainRepo = groupDomainRepo;
        this.jdPostRepo = jdPostRepo;
        this.jdPostApprovalRepo = jdPostApprovalRepo;
        this.jdPostSkillRepo = jdPostSkillRepo;
        this.jdPostLevelRepo = jdPostLevelRepo;
        this.provinceJdPostRepo = provinceJdPostRepo;
        this.levelRepo = levelRepo;
        this.skillRepo = skillRepo;
        this.provinceRepo = provinceRepo;
        this.domainRepo = domainRepo;
        this.applicantDetailRepo = applicantDetailRepo;
        this.applicantLevelRepo = applicantLevelRepo;
        this.applicantSkillRepo = applicantSkillRepo;
        this.applicantProvinceRepo = applicantProvinceRepo;
        this.databaseContext = databaseContext;
    }

    public async Task<RecruiterDetail> GetRecruiterDetail(int userId)
    {
        var user = await this.userRepo.FindOneAsync(us => us.Id == userId);

        if (user == default)
        {
            throw new Exception("errUserNotFound");
        }

        var recruitmentDetail = await this.recruiterDetailRepo.FindOneAsync(us => us.UserId == userId);

        return recruitmentDetail;
    }

    public async Task<IList<GroupDomainRecruiterDto>> GetGroupDomainRecruiter(int userId)
    {
        var groupDomains = await this.domainService.GetAllGroupDomain();
        var recruitmentDomains = await this.recruiterDomainRepo.FindManyAsync(us => us.UserId == userId);
        var result = new List<GroupDomainRecruiterDto>();
        var selected = false;

        foreach (var groupDomain in groupDomains)
        {
            selected = recruitmentDomains.Any(us => us.GroupDomainId == groupDomain.Id);
            result.Add(new GroupDomainRecruiterDto(groupDomain, selected));
        }
        return result;
    }

    public async Task<IList<RecruiterDomain>> UpdateRecruiterGroupDomain(UpdateRecruiterGroupDomainDto requestDto)
    {
        if (requestDto.UserId == 0)
        {
            throw new Exception("errUserNotFound");
        }

        var groupDomains = await this.groupDomainRepo.FindManyAsync(us => requestDto.GetGroupDomainIds().Contains(us.Id));

        if (groupDomains.Count != requestDto.GroupDomains.Count)
        {
            throw new Exception("errGroupDomainNotFound");
        }

        var olderGroupDomains = await this.recruiterDomainRepo.FindManyAsync(us => us.UserId == requestDto.UserId);
        var removeGroupDomains = olderGroupDomains.ExceptBy(groupDomains.Select(us => us.Id), os => os.GroupDomainId).ToList();
        var addGroupDomains = groupDomains.ExceptBy(olderGroupDomains.Select(us => us.GroupDomainId), os => os.Id).ToList();

        try
        {
            if (removeGroupDomains != null)
            {
                this.recruiterDomainRepo.Remove(removeGroupDomains);
            }

            if (addGroupDomains != null)
            {
                foreach (var groupDomain in addGroupDomains)
                {
                    var recruitmentGroupDomain = new RecruiterDomain
                    {
                        UserId = requestDto.UserId,
                        GroupDomainId = groupDomain.Id,
                    };

                    await this.recruiterDomainRepo.SaveOneAsync(recruitmentGroupDomain);
                }
            }

            return await this.recruiterDomainRepo.FindManyAsync(us => us.UserId == requestDto.UserId);
        }
        catch
        {
            throw new Exception("errUpdateGroupDomainFailed");
        }
    }

    public async Task<bool> UpdateRecruiterBasicInfo(UpdateRecruiterBasicInfoDto requestDto, string imagePathFolder, int userId)
    {
        var user = await this.userRepo.FindOneAsync(us => us.Id == userId);

        if (user == default)
        {
            throw new Exception("errUserNotFound");
        }

        var recruitment = await this.recruiterDetailRepo.FindOneAsync(us => us.UserId == userId);

        if (recruitment == default)
        {
            throw new Exception("errRecruitmentInfoNotFound");
        }

        var transaction = this.databaseContext.Database.BeginTransaction();

        try
        {
            var recruitmentUpdate = RecruiterMapper.ToUpdateModel(recruitment, requestDto);
            await this.recruiterDetailRepo.UpdateOneAsync(recruitmentUpdate);
            await transaction.CommitAsync();

            if (recruitmentUpdate.Avatar != requestDto.OldAvatar)
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

    public async Task<IList<RecruiterJobPostListDto>> GetRecruiterJobPostsAsync(int userId)
    {
        var jobPosts = await this.jdPostRepo.FindManyAsync(us => us.UserId == userId && us.IsDelete != EDelete.Delete);

        var jobPostDtos = new List<RecruiterJobPostListDto>();

        foreach (var jobPost in jobPosts)
        {
            var statusApprovalJob = (await this.jdPostApprovalRepo.FindManyAsync(us => us.JdPostId == jobPost.Id)).OrderByDescending(x => x.Id).FirstOrDefault();

            var jobPostDto = new RecruiterJobPostListDto
            {
                JobPostId = jobPost.Id,
                Title = jobPost.Title,
                WorkingType = jobPost.WorkingType,
                EndDate = jobPost.EndDate,
                Status = jobPost.Status,
                StatusApproval = statusApprovalJob != null ? statusApprovalJob.Status : null,
            };

            jobPostDtos.Add(jobPostDto);
        }

        return jobPostDtos;

    }

    public async Task<bool> CreateJobPost(AddRecruiterJdDto requestDto)
    {
        var transaction = this.databaseContext.Database.BeginTransaction();

        try
        {
            var jdPost = JdPostMapper.ToCreateModel(requestDto);
            await this.jdPostRepo.SaveOneAsync(jdPost);

            if (requestDto.Skills?.Any() == true)
            {
                var jdPostSkills = requestDto.Skills.Select(skillId => new JdPostSkill
                {
                    SkillId = skillId,
                    JdPostId = jdPost.Id,
                });

                await this.jdPostSkillRepo.SaveManyAsync(jdPostSkills);
            }

            if (requestDto.Levels?.Any() == true)
            {
                var jdPostLevels = requestDto.Levels.Select(levelId => new JdPostLevel
                {
                    LevelId = levelId,
                    JdPostId = jdPost.Id,
                });

                await this.jdPostLevelRepo.SaveManyAsync(jdPostLevels);
            }

            if (requestDto.Provinces?.Any() == true)
            {
                var jdPostProvinces = requestDto.Provinces.Select(provinceId => new ProvinceJdPost
                {
                    ProvinceId = provinceId,
                    JdPostId = jdPost.Id,
                });

                await this.provinceJdPostRepo.SaveManyAsync(jdPostProvinces);
            }

            var jdPostStatusApproval = new JdPostApproval
            {
                JdPostId = jdPost.Id,
                Status = EJdPostApproval.PendingApproval
            };

            await this.jdPostApprovalRepo.SaveOneAsync(jdPostStatusApproval);

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> UpdateJobPost(UpdateRecruiterJdDto requestDto)
    {
        var jdPostDB = await this.jdPostRepo.FindOneAsync(us => us.Id == requestDto.JdPostId);

        var transaction = this.databaseContext.Database.BeginTransaction();

        try
        {
            var jdPost = JdPostMapper.ToUpdateModel(requestDto, jdPostDB);
            await this.jdPostRepo.UpdateOneAsync(jdPost);

            if (requestDto.Skills.Count == 0)
            {
                var removeJdSkills = await this.jdPostSkillRepo.FindManyAsync(us => us.JdPostId == requestDto.JdPostId);
                this.jdPostSkillRepo.Remove(removeJdSkills);
            }
            else
            {
                var olderSkills = await this.jdPostSkillRepo.FindManyAsync(us => us.JdPostId == requestDto.JdPostId);
                var requestSkills = await this.skillRepo.FindManyAsync(us => requestDto.Skills.Contains(us.Id));
                var removeSkills = olderSkills.ExceptBy(requestSkills.Select(us => us.Id), uq => uq.SkillId).ToList();
                var addSkills = requestSkills.ExceptBy(olderSkills.Select(us => us.SkillId), uq => uq.Id).ToList();

                if (removeSkills.Count != 0)
                {
                    this.jdPostSkillRepo.Remove(removeSkills);
                }

                if (addSkills.Count != 0)
                {
                    var jdSkills = addSkills.Select(skill => new JdPostSkill
                    {
                        SkillId = skill.Id,
                        JdPostId = requestDto.JdPostId,
                    });

                    await this.jdPostSkillRepo.SaveManyAsync(jdSkills);
                }
            }

            if (requestDto.Levels.Count != 0)
            {
                var olderLevels = await this.jdPostLevelRepo.FindManyAsync(us => us.JdPostId == requestDto.JdPostId);
                var requestLevels = await this.levelRepo.FindManyAsync(us => requestDto.Levels.Contains(us.Id));
                var removeLevels = olderLevels.ExceptBy(requestLevels.Select(us => us.Id), uq => uq.LevelId).ToList();
                var addLevels = requestLevels.ExceptBy(olderLevels.Select(us => us.LevelId), uq => uq.Id).ToList();

                if (removeLevels.Count != 0)
                {
                    this.jdPostLevelRepo.Remove(removeLevels);
                }

                if(addLevels.Count != 0)
                {
                    var jdLevels = addLevels.Select(level => new JdPostLevel
                    {
                        LevelId = level.Id,
                        JdPostId = requestDto.JdPostId
                    });

                    await this.jdPostLevelRepo.SaveManyAsync(jdLevels);
                }
            }

            if(requestDto.Provinces.Count != 0)
            {
                var olderProvinces = await this.provinceJdPostRepo.FindManyAsync(us => us.JdPostId == requestDto.JdPostId);
                var requestLevels = await this.provinceRepo.FindManyAsync(us => requestDto.Provinces.Contains(us.Id));
                var removeProvinces = olderProvinces.ExceptBy(requestLevels.Select(us => us.Id), uq => uq.ProvinceId).ToList();
                var addProvinces = requestLevels.ExceptBy(olderProvinces.Select(us => us.ProvinceId), uq => uq.Id).ToList();

                if (removeProvinces.Count != 0)
                {
                    this.provinceJdPostRepo.Remove(removeProvinces);
                }

                if (addProvinces.Count != 0)
                {
                    var jdProvinces = addProvinces.Select(provinces => new ProvinceJdPost
                    {
                        ProvinceId = provinces.Id,
                        JdPostId = requestDto.JdPostId
                    }); 

                    await this.provinceJdPostRepo.SaveManyAsync(jdProvinces);
                }
            }

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<ViewJobPostPaginationDto> GetJobPostPublicByRecruiter(QueryListJobPostDto query, int userId)
    {
        var jobPosts = await (from p in this.jdPostRepo.Entities
                              join a in this.jdPostApprovalRepo.Entities on p.Id equals a.JdPostId
                              join jl in this.jdPostLevelRepo.Entities on p.Id equals jl.JdPostId
                              join jp in this.provinceJdPostRepo.Entities on p.Id equals jp.JdPostId
                              where p.IsDelete == EDelete.NoDelete
                              where a.Id == this.jdPostApprovalRepo.Entities
                                  .Where(a2 => a2.JdPostId == p.Id)
                                  .Max(a2 => a2.Id)
                              where a.Status == 0
                              where p.UserId == userId
                              where (string.IsNullOrEmpty(query.Title) || p.Title.ToLower().Contains(query.Title.ToLower()))
                                  && (query.Levels == default || query.Levels.Contains(jl.LevelId))
                                  && (query.Provinces == default || query.Provinces.Contains(jp.ProvinceId))
                              select p).Distinct().ToListAsync();

        var totalRecord = jobPosts.Count();

        var jobPostDtos = new List<JobPostResponseDto>();

        foreach (var job in jobPosts)
        {
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
                WorkingType = job.WorkingType
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

    public async Task<IList<RecruiterPublicDto>> GetRecruitersIndex()
    {

        var recruiterUsers = await (
            from r in this.recruiterDetailRepo.Entities
            join u in this.userRepo.Entities on r.UserId equals u.Id
            where u.Status != EUserStatus.InActive
            select new
            {
                Recruiter = r,
                User = u
            }
        ).ToListAsync();

        var jobCountByRecruiter = await (
            from p in jdPostRepo.Entities
            join a in jdPostApprovalRepo.Entities on p.Id equals a.JdPostId
            where p.IsDelete == EDelete.NoDelete
                  && a.Id == jdPostApprovalRepo.Entities
                     .Where(a2 => a2.JdPostId == p.Id)
                     .Max(a2 => a2.Id)
                  && a.Status == 0
            group p by p.UserId into g
            select new
            {
                RecruiterId = g.Key,
                JobCount = g.Count()
            }
        ).ToDictionaryAsync(x => x.RecruiterId, x => x.JobCount);

        var topRecruiters = recruiterUsers
            .Select(x => new
            {
                Recruiter = x.Recruiter,
                User = x.User,
                JobCount = jobCountByRecruiter.TryGetValue(x.Recruiter.UserId, out var count) ? count : 0
            })
            .OrderByDescending(x => x.JobCount)
            .ThenByDescending(x => x.Recruiter.CreatedAt) 
            .Take(6)
            .ToList();

        var recruiterPublicsDto = topRecruiters.Select(x => new RecruiterPublicDto
        {
            RecruiterId = x.Recruiter.Id,
            JobPostPuclic = x.JobCount, 
            RecruiterName = x.Recruiter.Name,
            RecuiterAvatar = x.Recruiter.Avatar,
            CompanySize = x.Recruiter.CompanySize,
            WorkSchedule = x.Recruiter.WorkSchedule,
            UserId = x.Recruiter.UserId,
            Status = x.User.Status,
            VerifyStatus = x.User.VerifyStatus,
        }).ToList();

        return recruiterPublicsDto;
    }


    public async Task<ViewRecruiterPaginationDto> GetRecruiters(QueryRecruiterPublicDto query)
    {
        var recruiters = await (from r in this.recruiterDetailRepo.Entities
                                join u in this.userRepo.Entities on r.UserId equals u.Id
                                join rgd in this.recruiterDomainRepo.Entities on r.UserId equals rgd.UserId into rgdJoin
                                from rgd in rgdJoin.DefaultIfEmpty()
                                where u.Status != EUserStatus.InActive
                                where (string.IsNullOrEmpty(query.Name) || r.Name.ToLower().Contains(query.Name.ToLower()))
                                    && (query.GroupDomains == default || query.GroupDomains.Contains(rgd.GroupDomainId))
                                select r).Distinct().ToListAsync();

        var totalRecrod = recruiters.Count();

        var recruiterPublicsDto = new List<RecruiterPublicDto>();

        var jobCountByRecruiter = await (
            from p in jdPostRepo.Entities
            join a in jdPostApprovalRepo.Entities on p.Id equals a.JdPostId
            where p.IsDelete == EDelete.NoDelete
            && a.Id == jdPostApprovalRepo.Entities
            .Where(a2 => a2.JdPostId == p.Id)
            .Max(a2 => a2.Id)
            && a.Status == 0
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

        var recruitersSkip = recruiterPublicsDto.Skip(query.GetSkip()).Take(query.PageSize);

        var pagination = new PaginationDto(query.Page, query.PageSize, totalRecrod);

        return new ViewRecruiterPaginationDto(recruitersSkip, pagination);
    }

    public async Task<IList<JobStatusStatisticDto>> GetJobStatusStatisticByRecruiter(int userId)
    {
        var approvals = await (
            from a in this.jdPostApprovalRepo.Entities
            join j in this.jdPostRepo.Entities on a.JdPostId equals j.Id
            join r in this.recruiterDetailRepo.Entities on j.UserId equals r.UserId
            where r.UserId == userId
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

    public async Task<IList<ApplicantAdminDto>> GetApplicantsPublic(QueryApplicantPublicDto query)
    {
        var applicants = await (from a in this.applicantDetailRepo.Entities
                                join u in this.userRepo.Entities on a.UserId equals u.Id
                                join al in this.applicantLevelRepo.Entities on a.UserId equals al.UserId into alGroup
                                from al in alGroup.DefaultIfEmpty()
                                join ap in this.applicantProvinceRepo.Entities on a.UserId equals ap.UserId into apGroup
                                from ap in apGroup.DefaultIfEmpty()
                                join ask in this.applicantSkillRepo.Entities on a.UserId equals ask.UserId into askGroup
                                from ask in askGroup.DefaultIfEmpty()
                                where u.Status == EUserStatus.Active
                                where (string.IsNullOrEmpty(query.Name) || a.Name.ToLower().Contains(query.Name.ToLower()))
                                    && (query.Levels == default || al != null && query.Levels.Contains(al.LevelId))
                                    && (query.Provinces == default || ap != null && query.Provinces.Contains(ap.ProvinceId))
                                    && (query.Skills == default || ask != null && query.Skills.Contains(ask.SkillId))
                                select a).Distinct().ToListAsync();

        var applicantsDto = new List<ApplicantAdminDto>();

        foreach(var applicant in applicants)
        {
            var user = await this.userRepo.FindOneAsync(us => us.Id == applicant.UserId);

            var applicantDto = new ApplicantAdminDto
            {
                Id = applicant.Id,
                UserId = user.Id,
                Status = user.Status,
                Name = applicant.Name,
                Avatar = applicant.Avatar,
                Email = applicant.Email,
            };

            applicantsDto.Add(applicantDto);
        }

        return applicantsDto;
    }
}

