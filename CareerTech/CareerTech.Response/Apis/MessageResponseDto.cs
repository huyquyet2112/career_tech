namespace CareerTech.Response.Apis;

public class MessageResponseDto(string message)
{
    public string Message { get; set; } = message;
}
