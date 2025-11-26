namespace Homework;

public class ToDoUser
{
    public Guid UserId { get; }
    public long TelegramUserId { get; set; }
    public string TelegramUserName { get; set; }
    public DateTime RegisteredAt { get; }

    public ToDoUser(string telegramUserName, long telegramUserId)
    {
        TelegramUserName = telegramUserName;
        TelegramUserId = telegramUserId;
        UserId =  Guid.NewGuid();
        RegisteredAt = DateTime.UtcNow;
    }
}
