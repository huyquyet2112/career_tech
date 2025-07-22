namespace CareerTech.Model.Entities;

public enum  EWorkSchedule
{
    MondayToFriday = 0,   
    MondayToSaturday = 1
}

public static class WorkScheduleHelper
{
    public static string ToLabel(this EWorkSchedule schedule)
    {
        return schedule switch
        {
            EWorkSchedule.MondayToFriday => "Monday - Friday",
            EWorkSchedule.MondayToSaturday => "Monday - Saturday",
            _ => "Unknow"
        };
    }
}
