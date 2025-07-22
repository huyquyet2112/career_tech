using CareerTech.Model.Context;
using CareerTech.Repo.Interfaces;
using CareerTech.Service.Interfaces;
using CareerTech.Model.Entities;
using Microsoft.EntityFrameworkCore;
using CareerTech.Response.Levels;
using CareerTech.Request.Applicants;
using System.Runtime.CompilerServices;

namespace CareerTech.Service.Services;

public class LevelService : ILevelService
{
    private readonly ILevelRepo levelRepo;
    private readonly IJdPostLevelRepo jdPostLevelRepo;
    private readonly IApplicantLevelRepo applicantLevelRepo;
    private readonly IUserRepo userRepo;
    private readonly DatabaseContext databaseContext;

    public LevelService(
        ILevelRepo levelRepo,
        IJdPostLevelRepo jdPostLevelRepo,
        IApplicantLevelRepo applicantLevelRepo,
        IUserRepo userRepo,
        DatabaseContext databaseContext)
    {
        this.levelRepo = levelRepo;
        this.jdPostLevelRepo = jdPostLevelRepo;
        this.userRepo = userRepo;
        this.applicantLevelRepo = applicantLevelRepo;
        this.databaseContext = databaseContext;
    }

    public async Task<IList<Level>> GetLevels()
    {
        return await this.levelRepo.FindManyAsync(us => true);
    }

    public async Task<IList<LevelResponseDto>> GetJdLevels(int? jdPostId)
    {
        var levels = await this.GetLevels();
        var jdLevelDtos = new List<LevelResponseDto>();
        var jdLevels = jdPostId != null ? await this.jdPostLevelRepo.FindManyAsync(us => us.JdPostId == jdPostId) : new List<JdPostLevel>();

        foreach (var level in levels)
        {
            bool selected = jdLevels.Any(us => us.LevelId == level.Id);
            var jdLevelDto = new LevelResponseDto(level, selected);
            jdLevelDtos.Add(jdLevelDto);
        }

        return jdLevelDtos;
    }

    public async Task<IList<LevelResponseDto>> GetApplicantLevels(int userId)
    {
        var levels = await this.GetLevels();
        var applicantLevelsDto = new List<LevelResponseDto>();
        var applicantLevels = await this.applicantLevelRepo.FindManyAsync(us => us.UserId == userId);

        foreach (var level in levels)
        {
            bool selected = applicantLevels.Any(us => us.LevelId == level.Id);
            var applicantLevelDto = new LevelResponseDto(level, selected);
            applicantLevelsDto.Add(applicantLevelDto);
        }

        return applicantLevelsDto;
    }

    public async Task<bool> UpdateApplicantLevel(UpdateApplicantLevelDto requestDto)
    {
        var user = await this.userRepo.FindOneAsync(us => us.Id == requestDto.UserId);

        if (user == default)
        {
            throw new Exception("errUserNotFound");
        }

        var transaction = this.databaseContext.Database.BeginTransaction();

        try
        {
            if(requestDto.Levels.Count == 0)
            {
                var removeLevels = await this.applicantLevelRepo.FindManyAsync(us => us.UserId == requestDto.UserId);
                this.applicantLevelRepo.Remove(removeLevels);
            }
            else
            {
                var olderLevels = await this.applicantLevelRepo.FindManyAsync(us => us.UserId == requestDto.UserId);
                var requestLevels = await this.levelRepo.FindManyAsync(us => requestDto.Levels.Contains(us.Id));
                var removeLevels = olderLevels.ExceptBy(requestLevels.Select(us => us.Id), uq => uq.LevelId).ToList();
                var addLevels = requestLevels.ExceptBy(olderLevels.Select(us => us.LevelId), uq => uq.Id).ToList();

                if(removeLevels.Count != 0)
                {
                    this.applicantLevelRepo.Remove(removeLevels);
                }

                if(addLevels.Count != 0)
                {
                    var applicantLevels = addLevels.Select(level => new ApplicantLevel
                    {
                        LevelId = level.Id,
                        UserId = requestDto.UserId,
                    });

                    await this.applicantLevelRepo.SaveManyAsync(applicantLevels);
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
}
