namespace Homework;

public class InMemoryUserRepository : IUserRepository
{
    private List<ToDoUser> userList = new();
    
    public async Task<ToDoUser>? GetUser(Guid userId, CancellationToken ct)
    {
        return userList.Find(item => item.UserId == userId);
    }

    public async Task<ToDoUser>? GetUserByTelegramUserId(long telegramUserId, CancellationToken ct)
    {
        return userList.Find(item => item.TelegramUserId == telegramUserId);
    }

    public async Task Add(ToDoUser user, CancellationToken ct)
    {
        userList.Add(user);
    }
}