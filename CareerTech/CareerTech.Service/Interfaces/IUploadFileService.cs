using CareerTech.Model.Entities;

namespace CareerTech.Service.Interfaces;

public interface IUploadFileService
{
    Task<CvFile> AddFile(int userId, string fileName, string urlFile);

    Task<bool> DeleteFile(int fileId);
}
