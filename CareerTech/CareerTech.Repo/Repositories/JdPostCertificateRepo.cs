using CareerTech.Model.Context;
using CareerTech.Model.Entities;
using CareerTech.Repo.Interfaces;

namespace CareerTech.Repo.Repositories;

public class JdPostCertificateRepo : AbstractRepo<JdPostCertificate>, IJdPostCertificateRepo
{
    public JdPostCertificateRepo(DatabaseContext dbContext) : base(dbContext)
    {

    }
}
