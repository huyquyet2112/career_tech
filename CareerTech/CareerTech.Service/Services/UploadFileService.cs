using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;
using CareerTech.Service.Interfaces;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTech.Service.Services;

public class UploadFileService : IUploadFileService
{
    private readonly DatabaseContext databaseContext;
    private readonly ICvFileRepo CVFileRepo;

    public UploadFileService(
        DatabaseContext databaseContext,
        ICvFileRepo cVFileRepo)
    {
        this.databaseContext = databaseContext;
        CVFileRepo = cVFileRepo;
    }

    public async Task<CvFile> AddFile(int userId, string fileName, string urlFile)
    {
        var existingFileNames = (await CVFileRepo.FindManyAsync(f => f.UserId == userId))
                        .Select(f => f.Name)
                        .ToList();

        var safeFileName = GetSafeFileName(fileName, existingFileNames);

        var fileCV = new CvFile
        {
            Name = safeFileName,
            UserId = userId,
            UrlFile = urlFile
        };

        using (var transaction = await this.databaseContext.Database.BeginTransactionAsync())
        {
            try
            {
                await this.CVFileRepo.SaveOneAsync(fileCV);
                await transaction.CommitAsync();
                return fileCV;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("msgErrorSavingFile", ex);
            }
        }
    }

    public async Task<bool> DeleteFile(int fileId)
    {
        try
        {
            var fileCv = await this.CVFileRepo.FindOneAsync(us => us.Id == fileId);

            fileCv.UserId = null;

            await this.CVFileRepo.UpdateOneAsync(fileCv);

            return true;
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }

    private string GetSafeFileName(string originalFileName, IEnumerable<string> fileNames)
    {
        string baseName = Path.GetFileNameWithoutExtension(originalFileName);
        string extension = Path.GetExtension(originalFileName);
        string newName = originalFileName;
        int i = 1;

        while (fileNames.Contains(newName, StringComparer.OrdinalIgnoreCase))
        {
            newName = $"{baseName}_{i}{extension}";
            i++;
        }

        return newName;
    }
}
