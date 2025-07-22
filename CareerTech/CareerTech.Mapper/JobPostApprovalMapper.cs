using CareerTech.Model.Entities;
using CareerTech.Request.JobPosts;

namespace CareerTech.Mapper;

public static class JobPostApprovalMapper
{
    public static JdPostApproval ToCreateModel(AddJobPostApprovalDto requestDto)
    {
        return new JdPostApproval
        {
            JdPostId = requestDto.JobPostId,
            UserId = requestDto.UserId,
            Status = requestDto.Status,
            Reason = requestDto.Reason <= 0 ? null : requestDto.Reason,
        };
    }

    public static JdPostApproval ToUpdateModel(UpdateJobPostApprovalDto updateDto, JdPostApproval jobPostApproval)
    {
        jobPostApproval.UserId = updateDto.UserId;
        jobPostApproval.Status = updateDto.Status;
        jobPostApproval.Reason = updateDto.Reason <= 0 ? null : updateDto.Reason;

        return jobPostApproval;
    }
}
