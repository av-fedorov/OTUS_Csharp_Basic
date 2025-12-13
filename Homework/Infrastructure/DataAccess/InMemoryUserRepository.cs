namespace Homework;

public class InMemoryUserRepository : IUserRepository
{
    private List<ToDoUser> userList = new();
    
    public ToDoUser? GetUser(Guid userId)
    {
        return userList.Find(item => item.UserId == userId);
    }

    public ToDoUser? GetUserByTelegramUserId(long telegramUserId)
    {
        return userList.Find(item => item.TelegramUserId == telegramUserId);
    }

    public void Add(ToDoUser user)
    {
        userList.Add(user);
    }
}