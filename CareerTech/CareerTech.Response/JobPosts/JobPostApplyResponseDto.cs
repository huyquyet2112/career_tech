using CareerTech.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTech.Response.JobPosts;

public class JobPostApplyResponseDto
{
    public int ApplyId { get; set; }

    required public string NameApplicant { get; set; }

    required public string JobTitle { get; set; }

    public string? Description {  get; set; }

    public EFitApplyJob? FitStatus { get; set; }
}
