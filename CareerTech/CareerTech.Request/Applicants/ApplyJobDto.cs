using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTech.Request.Applicants;

public class ApplyJobDto
{
    required public int UserId { get; set; }

    required public int JobPostId { get; set; }

    public string? Description { get; set; }

    public int FileId { get; set; }

    public IFormFile? FileCV { get; set; }
}
