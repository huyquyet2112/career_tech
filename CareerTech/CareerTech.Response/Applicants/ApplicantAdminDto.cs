using CareerTech.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerTech.Response.Applicants;

public class ApplicantAdminDto
{
    required public int Id { get; set; }

    required public string Name { get; set; }

    public string? Avatar { get; set; }

    required public int UserId { get; set; }

    public string? Email { get;set; }

    required public EUserStatus Status { get;set; }
}
