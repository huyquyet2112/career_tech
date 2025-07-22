using CareerTech.Mapper;
using CareerTech.Model.Entities;
using CareerTech.Model.Enums;
using CareerTech.Repo.Interfaces;
using CareerTech.Request.JobPosts;
using CareerTech.Response.JobPosts;
using CareerTech.Response.Paginations;
using CareerTech.Response.Recruitments;
using CareerTech.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CareerTech.Service.Services;

public class JdPostService : IJdPostService
{
    private readonly IJdPostRepo jdPostRepo;
    private readonly IJdPostApprovalRepo jdPostApprovalRepo;
    private readonly IRecruiterDetailRepo recruiterDetailRepo;
    private readonly IAdminDetailRepo adminDetailRepo;
    private readonly IJdPostLevelRepo jdPostLevelRepo;
    private readonly IProvincesJdPostRepo provincesJdPostRepo;
    private readonly ICvFileRepo cvFileRepo;
    private readonly ILevelRepo levelRepo;
    private readonly IProvincesRepo provincesRepo;
    private readonly IDomainRepo domainRepo;
    private readonly IApplyJobRepo applyJobRepo;
    private readonly IApplicantDetailRepo applicantDetailRepo;
    private readonly ISavedJdPostRepo savedJdPostRepo;
    private readonly IJdPostSkillRepo jdSkillRepo;
    private readonly IUserRepo userRepo;


    public JdPostService(
        IJdPostRepo jdPostRepo,
        IJdPostApprovalRepo jdPostApprovalRepo,
        IRecruiterDetailRepo recruiterDetailRepo,
        IAdminDetailRepo adminDetailRepo,
        IJdPostLevelRepo jdPostLevelRepo,
        IProvincesJdPostRepo provinceJdPostRepo,
        ILevelRepo levelRepo,
        IProvincesRepo provincesRepo,
        IDomainRepo domainRepo,
        ICvFileRepo cvFileRepo,
        IApplyJobRepo applyJobRepo,
        IApplicantDetailRepo applicantDetailRepo,
        ISavedJdPostRepo savedJdPostRepo,
        IJdPostSkillRepo jdSkillRepo,
        IUserRepo userRepo)
    {
        this.jdPostRepo = jdPostRepo;
        this.jdPostApprovalRepo = jdPostApprovalRepo;
        this.recruiterDetailRepo = recruiterDetailRepo;
        this.adminDetailRepo = adminDetailRepo;
        this.jdPostLevelRepo = jdPostLevelRepo;
        this.provincesJdPostRepo = provinceJdPostRepo;
        this.levelRepo = levelRepo;
        this.provincesRepo = provincesRepo;
        this.domainRepo = domainRepo;
        this.applyJobRepo = applyJobRepo;
        this.cvFileRepo = cvFileRepo;
        this.applicantDetailRepo = applicantDetailRepo;
        this.savedJdPostRepo = savedJdPostRepo;
        this.jdSkillRepo = jdSkillRepo;
        this.userRepo = userRepo;
    }

    public async Task<JdPost> GetJobPostById(int id)
    {
        return await this.jdPostRepo.FindOneAsync(us => us.Id == id);
    }

    public async Task<IList<JobPostStatusApprovalDto>> GetJobApprovalsById(int jobPostId)
    {
        var jobPosts = await this.jdPostApprovalRepo.FindManyAsync(us => us.JdPostId == jobPostId);
        var jobPostDtos = new List<JobPostStatusApprovalDto>();

        foreach (var jobPost in jobPosts)
        {
            string? userName = null;

            if (jobPost.UserId != null)
            {
                userName = (await this.adminDetailRepo.FindOneAsync(us => us.UserId == jobPost.UserId)).Name;
            }

            var jobPostDto = new JobPostStatusApprovalDto
            {
                Id = jobPost.Id,
                Status = jobPost.Status,
                Reason = jobPost.Reason,
                UserApproval = userName,
                CreatedAt = jobPost.CreatedAt,
                UpdatedAt = jobPost.UpdatedAt,
            };

            jobPostDtos.Add(jobPostDto);
        }

        return jobPostDtos;
    }

    public async Task<JdPostApproval> GetJobApprovalById(int jobPostApprovalId)
    {
        var jobPostApproval = await this.jdPostApprovalRepo.FindOneAsync(us => us.Id == jobPostApprovalId);

        return jobPostApproval;
    }

    public async Task<bool> UpdateJobPostApproval(UpdateJobPostApprovalDto requestDto)
    {
        try
        {
            var jobPostApproval = await this.jdPostApprovalRepo.FindOneAsync(us => us.Id == requestDto.JobPostApprovalId);

            var updatejobPostApproval = JobPostApprovalMapper.ToUpdateModel(requestDto, jobPostApproval);

            await this.jdPostApprovalRepo.UpdateOneAsync(updatejobPostApproval);

            return true;
        }
        catch
        {
            throw;
        }
    }

    public async Task<ViewJobPostPaginationDto> GetJobPostPublic(QueryListJobPostDto query)
    {
        var jobPosts = await (from p in this.jdPostRepo.Entities
                              join a in this.jdPostApprovalRepo.Entities on p.Id equals a.JdPostId
                              join jl in this.jdPostLevelRepo.Entities on p.Id equals jl.JdPostId
                              join jp in this.provincesJdPostRepo.Entities on p.Id equals jp.JdPostId
                              join jsk in this.jdSkillRepo.Entities on p.Id equals jsk.JdPostId into jskGroup
                              from jsk in jskGroup.DefaultIfEmpty()
                              where p.IsDelete == EDelete.NoDelete
                              where a.Id == this.jdPostApprovalRepo.Entities
                                  .Where(a2 => a2.JdPostId == p.Id)
                                  .Max(a2 => a2.Id)
                              where a.Status == 0
                              where (string.IsNullOrEmpty(query.Title) || p.Title.ToLower().Contains(query.Title.ToLower()))
                                 && ((query.WorkingType == default) || query.WorkingType.Contains(p.WorkingType))
                                 && ((query.Levels == default) || query.Levels.Contains(jl.LevelId))
                                 && ((query.Provinces == default) || query.Provinces.Contains(jl.LevelId))
                                 && ((query.Skills == default || query.Skills.Contains(jsk.SkillId)))
                              select p)

                     .Distinct().ToListAsync();


        var totalRecord = jobPosts.Count();

        var jobPostDtos = new List<JobPostResponseDto>();

        foreach (var job in jobPosts)
        {
            var jobPostLevels = await this.jdPostLevelRepo.FindManyAsync(us => us.JdPostId == job.Id);
            var jobPostProvinces = await this.provincesJdPostRepo.FindManyAsync(us => us.JdPostId == job.Id);
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
                var province = await this.provincesRepo.FindOneAsync(us => us.Id == jobPostProvince.ProvinceId);

                jobProvinceDto.Add(province);
            }

            jobPostDto.Province = jobProvinceDto;

            jobPostDtos.Add(jobPostDto);
        }

        var jobs = jobPostDtos.Skip(query.GetSkip()).Take(query.PageSize);

        var pagination = new PaginationDto(query.Page, query.PageSize, totalRecord);

        return new ViewJobPostPaginationDto(jobs, pagination);

    }

    public async Task<JobPostPublicDetailDto> GetDetailJobPost(int jobPostId, int userId)
    {
        var jobPost = await this.jdPostRepo.FindOneAsync(us => us.Id == jobPostId);

        bool isApplied = false;
        bool isSaved = false;

        if (userId != 0)
        {
            var appliedJob = await applyJobRepo.FindManyAsync(us =>
                us.JdPostId == jobPostId && us.UserId == userId
            );

            var savedJob = await this.savedJdPostRepo.FindOneAsync(us => us.JdPostId == jobPostId && us.UserId == userId);

            if (appliedJob != null && appliedJob.Any())
            {
                isApplied = true;
            }

            if (savedJob != null)
            {
                isSaved = true;
            }
        }

        return new JobPostPublicDetailDto
        {
            Id = jobPost.Id,
            RecruiterId = jobPost.UserId,
            Title = jobPost.Title,
            DomainId = jobPost.DomainId,
            Description = jobPost.Description,
            Status = jobPost.Status,
            EndDate = jobPost.EndDate,
            MinSalary = jobPost.MinSalary,
            MaxSalary = jobPost.MaxSalary,
            CurrencySalary = jobPost.CurrencySalary,
            Quantity = jobPost.Quantity,
            IsDelete = jobPost.IsDelete,
            Requirement = jobPost.Requirement,
            Benefits = jobPost.Benefits,
            WorkingHours = jobPost.WorkingHours,
            WorkLocation = jobPost.WorkLocation,
            ExperienceYear = jobPost.ExperienceYear,
            IsApplied = isApplied,
            IsSaved = isSaved,
        };
    }

    public async Task<IList<JobPostApplicantsDto>> GetJobPostApplicants(int jobPostId)
    {
        var applyJobs = await this.applyJobRepo.FindManyAsync(us => us.JdPostId == jobPostId);

        var jobPostApplicantsDto = new List<JobPostApplicantsDto>();

        foreach (var applyJob in applyJobs)
        {
            var file = await this.cvFileRepo.FindOneAsync(us => us.Id == applyJob.CvFileId);
            var user = await this.applicantDetailRepo.FindOneAsync(us => us.UserId == applyJob.UserId);

            var jobPostApplicant = new JobPostApplicantsDto
            {
                UserId = applyJob.UserId,
                ApplyId = applyJob.Id,
                NameApplicant = user.Name,
                FileId = file.Id,
                FileName = file.Name,
                UrlFile = file.UrlFile,
                Status = applyJob.Status,
                FitApplyJob = applyJob.FitStatus,
                CreatedAt = applyJob.CreatedAt,
            };

            jobPostApplicantsDto.Add(jobPostApplicant);
        }

        return jobPostApplicantsDto;
    }

    public async Task<bool> UpdateStatusApplication(int applyId)
    {
        var applyJob = await this.applyJobRepo.FindOneAsync(us => us.Id == applyId);
        applyJob.Status = EApplyJobStatus.Viewed;
        applyJob.ViewedAt = DateTime.Now;
        await this.applyJobRepo.UpdateOneAsync(applyJob);

        return true;
    }

    public async Task<JobPostApplyResponseDto> GetDetailApplicantion(int applyId)
    {
        var applyJob = await this.applyJobRepo.FindOneAsync(us => us.Id == applyId);

        var user = await this.applicantDetailRepo.FindOneAsync(us => us.UserId == applyJob.UserId);

        var jobPost = await this.jdPostRepo.FindOneAsync(us => us.Id == applyJob.JdPostId);

        return new JobPostApplyResponseDto
        {
            ApplyId = applyJob.Id,
            NameApplicant = user.Name,
            JobTitle = jobPost.Title,
            Description = applyJob.Description,
            FitStatus = applyJob.FitStatus,
        };
    }

    public async Task<bool> UpdateFitStatusApplication(JobPostAppicantionFitStatusDto requestDto, int applyId)
    {
        try
        {
            var applyJob = await this.applyJobRepo.FindOneAsync(us => us.Id == applyId);

            applyJob.FitStatus = requestDto.FitStatus < 0 ? null : requestDto.FitStatus;
            applyJob.ReviewAt = requestDto.FitStatus < 0 ? null : DateTime.Now;

            await this.applyJobRepo.UpdateOneAsync(applyJob);

            return true;
        }
        catch (Exception)
        {
            throw new Exception();
        }

    }

    public async Task<bool> UpdateSavedJob(UpdateSavedJobDto requestDto)
    {
        var savedJob = await this.savedJdPostRepo.FindOneAsync(us => us.UserId == requestDto.UserId && us.JdPostId == requestDto.JobPostId);

        if (requestDto.IsSaved == true && savedJob != null)
        {

            this.savedJdPostRepo.Remove(savedJob);

        }
        else if (requestDto.IsSaved == false && savedJob == null)
        {
            var newSavedJob = new SavedJdPost
            {
                UserId = requestDto.UserId,
                JdPostId = requestDto.JobPostId,
            };
            await this.savedJdPostRepo.SaveOneAsync(newSavedJob);
        }

        return true;
    }

    public async Task<List<JobPostResponseDto>> GetJobPostPulicIndex()
    {
        var jobPosts = await (from j in this.jdPostRepo.Entities
                              join ja in this.jdPostApprovalRepo.Entities on j.Id equals ja.JdPostId
                              where ja.Id == this.jdPostApprovalRepo.Entities
                              .Where(ja2 => ja2.JdPostId == j.Id)
                              .Max(ja2 => ja2.Id)
                              where ja.Status == EJdPostApproval.Approved
                              select j).OrderByDescending(j => j.CreatedAt).Take(6).ToListAsync();

        var jobPostDtos = new List<JobPostResponseDto>();

        foreach (var job in jobPosts)
        {
            var jobPostLevels = await this.jdPostLevelRepo.FindManyAsync(us => us.JdPostId == job.Id);
            var jobPostProvinces = await this.provincesJdPostRepo.FindManyAsync(us => us.JdPostId == job.Id);
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
                var province = await this.provincesRepo.FindOneAsync(us => us.Id == jobPostProvince.ProvinceId);

                jobProvinceDto.Add(province);
            }

            jobPostDto.Province = jobProvinceDto;

            jobPostDtos.Add(jobPostDto);
        }

        return jobPostDtos;
    }

    public async Task<int> GetRecruiterIdByJobId(int jobJostId)
    {
        return (await this.jdPostRepo.FindOneAsync(us => us.Id == jobJostId)).UserId;
    }
}
