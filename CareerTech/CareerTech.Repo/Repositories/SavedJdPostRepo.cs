using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class SavedJdPostRepo : AbstractRepo<SavedJdPost>, ISavedJdPostRepo
{
    public SavedJdPostRepo(DatabaseContext dbContext): base(dbContext)
    {

    }
}
