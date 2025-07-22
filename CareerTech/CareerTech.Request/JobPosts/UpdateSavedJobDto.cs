namespace CareerTech.Request.JobPosts;

public class UpdateSavedJobDto
{
    public int JobPostId { get; set; }

    public int UserId { get; set; }

    public bool IsSaved { get; set; }
}
