using CareerTech.Model.Entities;

namespace CareerTech.Response.Levels;

public class LevelResponseDto(Level level, bool isSelected = false)
{
    public int Id { get; set; } = level.Id;

    public string Name { get; set; } = level.Name;

    public bool IsSelected { get; set; } = isSelected;
}
