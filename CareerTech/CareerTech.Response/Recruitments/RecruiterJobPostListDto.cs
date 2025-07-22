using CareerTech.Model.Enums;

namespace CareerTech.Response.Recruitments;

/// <summary>
/// WorkingType.
/// </summary>
public class RecruiterJobPostListDto
{
    /// <summary>
    /// Gets or sets JobPostId.
    /// </summary>
    public int JobPostId { get; set; }

    /// <summary>
    /// Gets or sets Title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets WorkingType.
    /// </summary>
    public EWorkingType? WorkingType { get; set; }

    /// <summary>
    /// Gets or sets EndDate.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets Status.
    /// </summary>
    public EJdPostStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets StatusApproval.
    /// </summary>
    public EJdPostApproval? StatusApproval { get; set; }
}
