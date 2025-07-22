using CareerTech.Model.Entities;
using CareerTech.Model.Enums;

namespace CareerTech.Response.Recruitments;

public class RecruitmentJdViewResponseDto(EModeView mode, JdPost? jr)
{
    public EModeView Mode { get; set; } = mode;

    public JdPost? JdPost { get; set; } = jr;
}
