using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;
using Microsoft.Identity.Client;

namespace CareerTech.Repo.Repositories;

public class ForgotPasswordRepo : AbstractRepo<ForgotPassword>, IForgotPasswordRepo
{
    public ForgotPasswordRepo(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
