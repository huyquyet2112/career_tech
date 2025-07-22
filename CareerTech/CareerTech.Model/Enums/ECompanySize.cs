namespace CareerTech.Model.Enums;

public enum ECompanySize
{
    Micro = 0,        
    VerySmall = 1,    
    Small = 2,        
    Medium = 3,       
    Large = 4,        
    Enterprise = 5    
}

public static class CompanySizeHelper
{
    public static string ToLabel(this ECompanySize size)
    {
        return size switch
        {
            ECompanySize.Micro => "Fewer than 10 employees",
            ECompanySize.VerySmall => "10 – 24 employees",
            ECompanySize.Small => "25 – 49 employees",
            ECompanySize.Medium => "50 – 199 employees",
            ECompanySize.Large => "200 – 499 employees",
            ECompanySize.Enterprise => "500+ employees",
            _ => "Unknown"
        };
    }
}
