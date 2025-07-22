using CareerTech.Model.Entities;
using CareerTech.Request.Applicants;
using CareerTech.Response.Levels;

namespace CareerTech.Service.Interfaces;

public interface ILevelService
{
    Task<IList<Level>> GetLevels();

    Task<IList<LevelResponseDto>> GetJdLevels(int? jdPostId);

    Task<IList<LevelResponseDto>> GetApplicantLevels(int userId);

    Task<bool> UpdateApplicantLevel(UpdateApplicantLevelDto requestDto);
}
