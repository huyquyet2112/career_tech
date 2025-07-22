using CareerTech.Mapper;
using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;
using CareerTech.Request.Applicants;
using CareerTech.Service.Interfaces;
using CareerTech.Model.Enums;
using CareerTech.Response.Applicants;
using CareerTech.Response.Paginations;
using Microsoft.EntityFrameworkCore;

namespace CareerTech.Service.Services;

public class ApplicantService : IApplicantService
{
    private readonly IUserRepo userRepo;
    private readonly IApplicantDetailRepo applicantDetailRepo;
    private readonly IRecruiterDetailRepo recruiterDetailRepo;
    private readonly IJdPostRepo jdPostRepo;
    private readonly ICvFileRepo cvFileRepo;
    private readonly IApplyJobRepo applyJobRepo;
    private readonly ISavedJdPostRepo savedJdPostRepo;
    private readonly IProvincesJdPostRepo jdProvinceRepo;
    private readonly IJdPostLevelRepo jdLevelRepo;
    private readonly DatabaseContext databaseContext;

    public ApplicantService(
        IUserRepo userRepo,
        IApplicantDetailRepo applicantDetailRepo,
        IRecruiterDetailRepo recruiterDetailRepo,
        IJdPostRepo jdPostRepo,
        ICvFileRepo cvFileRepo,
        IApplyJobRepo applyJobRepo,
        ISavedJdPostRepo savedJdPostRepo,
        IProvincesJdPostRepo jdProvinceRepo,
        IJdPostLevelRepo jdLevelRepo,
        DatabaseContext databaseContext)
    {
        this.userRepo = userRepo;
        this.applicantDetailRepo = applicantDetailRepo;
        this.recruiterDetailRepo = recruiterDetailRepo;
        this.cvFileRepo = cvFileRepo;
        this.jdPostRepo = jdPostRepo;
        this.applyJobRepo = applyJobRepo;
        this.savedJdPostRepo = savedJdPostRepo;
        this.jdProvinceRepo = jdProvinceRepo;
        this.jdLevelRepo = jdLevelRepo;
        this.databaseContext = databaseContext;
    }

    public async Task<ApplicantDetail> Detail(int userId)
    {
        var user = await this.userRepo.FindOneAsync(us => us.Id == userId);

        if (user == default)
        {
            throw new Exception("errUserNotFound");
        }

        var applicantDetail = await this.applicantDetailRepo.FindOneAsync(us => us.UserId == userId);

        return applicantDetail;
    }

    public async Task<IList<CvFile>> ApplicantCvFiles(int userId)
    {
        var user = await this.userRepo.FindOneAsync(us => us.Id == userId);

        if (user == default)
        {
            throw new Exception("errUserNotFound");
        }

        var cvFiles = await this.cvFileRepo.FindManyAsync(us => us.UserId == userId);

        return cvFiles;
    }

    public async Task<bool> UpdateApplicantBasicInfo(UpdateApplicantBasicInforDto requestDto, string imagePathFolder, int userId)
    {
        var user = await this.userRepo.FindOneAsync(us => us.Id == userId);

        if (user == default)
        {
            throw new Exception("errUserNotFound");
        }

        var applicantDetail = await this.applicantDetailRepo.FindOneAsync(us => us.UserId == userId);

        if (applicantDetail == default)
        {
            throw new Exception("errRecruitmentInfoNotFound");
        }

        var transaction = this.databaseContext.Database.BeginTransaction();

        try
        {
            var applicantUpdate = ApplicantDetailMapper.ToUpdateModel(applicantDetail, requestDto);
            await this.applicantDetailRepo.UpdateOneAsync(applicantUpdate);
            await transaction.CommitAsync();

            if (applicantUpdate.Avatar != requestDto.OldAvatar)
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

    public async Task<bool> ApplyJob(ApplyJobDto requestDto)
    {
        using (var transaction = await this.databaseContext.Database.BeginTransactionAsync())
        {
            try
            {
                var applyJob = new ApplyJob
                {
                    JdPostId = requestDto.JobPostId,
                    UserId = requestDto.UserId,
                    Status = EApplyJobStatus.Submitted,
                    Description = requestDto.Description,
                };

                if (requestDto.FileId == 0 && requestDto.FileCV != null)
                {

                    byte[] fileBytes = new byte[requestDto.FileCV.Length];

                    string fileData;

                    using (var stream = requestDto.FileCV.OpenReadStream())
                    {
                        await stream.ReadAsync(fileBytes, 0, (int)requestDto.FileCV.Length);
                        fileData = Convert.ToBase64String(fileBytes);
                    }

                    var cvFile = new CvFile
                    {
                        Name = requestDto.FileCV.FileName,
                        UserId = null,
                        UrlFile = fileData
                    };

                    await this.cvFileRepo.SaveOneAsync(cvFile);
                    applyJob.CvFileId = cvFile.Id;
                }
                else
                {
                    applyJob.CvFileId = requestDto.FileId;
                }
                await this.applyJobRepo.SaveOneAsync(applyJob);
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw new Exception();
            }
        }
    }

    public async Task<ViewApplicationJobPostDto> GetAppliedJobPosts(QueryApplicationJobPost query, int userId)
    {
        var appliedJob = (await this.applyJobRepo.FindManyAsync(us => us.UserId == userId))
            .GroupBy(x => x.JdPostId)
            .Select(g => g.OrderByDescending(x => x.CreatedAt).First()).ToList();

        var applicantAppliedJobPostsDto = new List<ApplicantAppliedJobPostDto>();

        var totalRecord = appliedJob.Count();

        foreach (var apply in appliedJob)
        {
            var jobPost = await this.jdPostRepo.FindOneAsync(us => us.Id == apply.JdPostId);
            var cvFile = await this.cvFileRepo.FindOneAsync(us => us.Id == apply.CvFileId);
            var recruiter = await this.recruiterDetailRepo.FindOneAsync(us => us.UserId == jobPost.UserId);

            var applicantAppliedJobPostDto = new ApplicantAppliedJobPostDto
            {
                RecruiterId = recruiter.UserId,
                RecruiterName = recruiter.Name,
                RecruiterAvatar = recruiter.Avatar,
                JobPostId = jobPost.Id,
                Title = jobPost.Title,
                MinSalary = jobPost.MinSalary,
                MaxSalary = jobPost.MaxSalary,
                CurrencySalary = jobPost.CurrencySalary,
                AppliedAt = apply.CreatedAt,
                UrlFile = cvFile.UrlFile,
                Status = apply.Status,
                ViewedAt = apply.ViewedAt,
                FitStatus = apply.FitStatus,
                ReviewedAt = apply.ReviewAt,
            };

            applicantAppliedJobPostsDto.Add(applicantAppliedJobPostDto);
        }

        var applicationJobPost = applicantAppliedJobPostsDto.Skip(query.GetSkip()).Take(query.PageSize);

        var pagination = new PaginationDto(query.Page, query.PageSize, totalRecord);

        return new ViewApplicationJobPostDto(applicationJobPost, pagination);
    }

    public async Task<ViewSaveJobPostDto> GetSavedJobPost(QueryApplicationJobPost query, int userId)
    {
        var savedJobPosts = await this.savedJdPostRepo.FindManyAsync(us => us.UserId == userId
        );

        var applicantSaveJobPostsDto = new List<ApplicantSavedJobPostDto>();

        var totalRecord = savedJobPosts.Count();

        foreach (var saveJob in savedJobPosts)
        {
            var jobPost = await this.jdPostRepo.FindOneAsync(us => us.Id == saveJob.JdPostId);
            var recruiter = await this.recruiterDetailRepo.FindOneAsync(us => us.UserId == jobPost.UserId);

            var applicantSaveJobPostDto = new ApplicantSavedJobPostDto
            {
                RecruiterId = recruiter.UserId,
                RecruiterAvatar = recruiter.Avatar,
                RecruiterName = recruiter.Name,
                JobPostId = jobPost.Id,
                Title = jobPost.Title,
                SavedId = saveJob.Id,
                MinSalary = jobPost.MinSalary,
                MaxSalary = jobPost.MaxSalary,
                CurrencySalary = jobPost.CurrencySalary,
                CreatedAt = saveJob.CreatedAt,
                EndDate = jobPost.EndDate,
            };

            applicantSaveJobPostsDto.Add(applicantSaveJobPostDto);
        }

        var saveJobPost = applicantSaveJobPostsDto.Skip(query.GetSkip()).Take(query.PageSize);
        var pagination = new PaginationDto(query.Page, query.PageSize, totalRecord);
        return new ViewSaveJobPostDto(saveJobPost, pagination);
    }

    public async Task<bool> DeleteSavedJob(int savedId)
    {
        var savedJobPost = await this.savedJdPostRepo.FindOneAsync(us => us.Id == savedId);

        this.savedJdPostRepo.Remove(savedJobPost);

        return true;
    }
}
