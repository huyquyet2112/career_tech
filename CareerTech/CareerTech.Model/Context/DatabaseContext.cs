using CareerTech.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerTech.Model.Context;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or  sets User.
    /// </summary>
    public DbSet<User> User { get; set; }

    /// <summary>
    /// Gets or sets Role.
    /// </summary>
    public DbSet<Role> Role { get; set; }

    /// <summary>
    /// Gets or sets Permission.
    /// </summary>
    public DbSet<Permission> Permission { get; set; }

    /// <summary>
    /// Gets or sets RolePermission.
    /// </summary>
    public DbSet<RolePermission> RolePermission { get; set; }

    /// <summary>
    /// Gets or sets UserRole.
    /// </summary>
    public DbSet<UserRole> UserRole { get; set; }

    /// <summary>
    /// Gets or sets RecruitmentDetail.
    /// </summary>
    public DbSet<RecruiterDetail> RecruitmentDetail { get; set; }

    /// <summary>
    /// Gets or sets JdPost.
    /// </summary>
    public DbSet<JdPost> JdPost { get; set; }

    /// <summary>
    /// Gets or sets CvFile.
    /// </summary>
    public DbSet<CvFile> CvFile { get; set; }

    /// <summary>
    /// Gets or sets CandidateDetail.
    /// </summary>
    public DbSet<ApplicantDetail> ApplicantDetail { get; set; }


    /// <summary>
    /// Gets or sets ApplyJob.
    /// </summary>
    public DbSet<ApplyJob> ApplyJob { get; set; }

    /// <summary>
    /// Gets or sets AdminDetail.
    /// </summary>
    public DbSet<AdminDetail> AdminDetail { get; set; }

    /// <summary>
    /// Gets or sets GroupDomain.
    /// </summary>
    public DbSet<GroupDomain> GroupDomain { get; set; }

    /// <summary>
    /// Gets or set Domain.
    /// </summary>
    public DbSet<Domain> Domain { get; set; }

    /// <summary>
    /// Gets or sets Level.
    /// </summary>
    public DbSet<Level> Level { get; set; }

    /// <summary>
    /// Gets or sets JdPostLevel.
    /// </summary>
    public DbSet<JdPostLevel> JdPostLevel { get; set; }

    /// <summary>
    /// Gets or sets Province.
    /// </summary>
    public DbSet<Province> Province { get; set; }

    /// <summary>
    /// Gets or sets ProvinceJdPost.
    /// </summary>
    public DbSet<ProvinceJdPost> ProvinceJdPost { get; set; }

    /// <summary>
    /// Gets or sets GroupSkill.
    /// </summary>
    public DbSet<GroupSkill> GroupSkill { get; set; }

    /// <summary>
    /// Gets or sets Skill.
    /// </summary>
    public DbSet<Skill> Skill { get; set; }

    /// <summary>
    /// Gets or sets Certificate.
    /// </summary>
    public DbSet<Certificate> Certificate { get; set; }

    /// <summary>
    /// Gets or sets ApplicantSkill.
    /// </summary>
    public DbSet<ApplicantSkill> ApplicantSkill { get; set; }

    /// <summary>
    /// Gets or sets ApplicantCertificate.
    /// </summary>
    public DbSet<ApplicantCertificate> ApplicantCertificate { get; set; }

    /// <summary>
    /// Gets or sets JdPostSkill.
    /// </summary>
    public DbSet<JdPostSkill> JdPostSkill { get; set; }

    /// <summary>
    /// Gets or sets JdPostCertificate.
    /// </summary>
    public DbSet<JdPostCertificate> JdPostCertificate { get; set; }

    /// <summary>
    /// Gets or sets RecruitmentDomain.
    /// </summary>
    public DbSet<RecruiterDomain> RecruitmentDomain { get; set; }

    /// <summary>
    /// Gets or sets JdPostApproval.
    /// </summary>
    public DbSet<JdPostApproval> JdPostApproval {  get; set; }

    /// <summary>
    /// Gets or sets SavedJdPost.
    /// </summary>
    public DbSet<SavedJdPost> SavedJdPost { get; set; }

    public DbSet<ForgotPassword> ForgotPassword { get; set; }

    public DbSet<ApplicantLevel> ApplicantLevel { get; set; }

    public DbSet<ApplicantProvince> ApplicantProvince {  get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<UserRole>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Role>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<RolePermission>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Permission>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<GroupDomain>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Domain>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Level>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<JdPost>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<JdPostLevel>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Province>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<ProvinceJdPost>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<CvFile>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<ApplyJob>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<AdminDetail>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<RecruiterDetail>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<ApplicantDetail>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<GroupSkill>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Skill>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Certificate>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<ApplicantSkill>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<ApplicantCertificate>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<JdPostSkill>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<JdPostCertificate>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<RecruiterDomain>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<JdPostApproval>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<SavedJdPost>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<ForgotPassword>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<ApplicantProvince>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<ApplicantLevel>().Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
