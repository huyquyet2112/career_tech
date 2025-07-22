using CareerTech.Model.Entities;

namespace CareerTech.Response.Provinces;

public class ProvinceResponseDto(Province province, bool isSelected = false)
{
    public int Id { get; set; } = province.Id;

    public string Name { get; set; } = province.Name;

    public bool isSelected = isSelected;
}
