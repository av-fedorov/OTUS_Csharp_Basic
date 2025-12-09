using Otus.ToDoList.ConsoleBot.Types;

namespace Homework;

public class UserService : IUserService
{   
    private static List<ToDoUser> userList = new List<ToDoUser>();
    public ToDoUser RegisterUser(long telegramUserId, string telegramUserName)
    {
        ToDoUser User = new ToDoUser(telegramUserName, telegramUserId);
        userList.Add(User);
        
        return User;
    }

    public ToDoUser? GetUser(long telegramUserId)
    {
        return userList.Find(item => item.TelegramUserId == telegramUserId);
    }
}