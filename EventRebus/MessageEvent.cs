namespace EventRebus;

public class MessageEvent
{
    public MessageEvent(string message)
    {
        Message = message;
    }
    public string  Message { get; set; }
    
}