using CareerTech.Model.Enums;

namespace CareerTech.Response.Admins;

public class JobStatusStatisticDto
{
    public EJdPostApproval StatusApproval { get; set; }

    public int Count { get; set; }
}
