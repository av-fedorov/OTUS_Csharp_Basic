using Otus.ToDoList.ConsoleBot.Types;

namespace Homework;

public class UserService : IUserService
{
    public ToDoUser RegisterUser(long telegramUserId, string telegramUserName)
    {
        ToDoUser User = new(telegramUserName, telegramUserId);
        return User;
    }

    public ToDoUser? GetUser(long telegramUserId)
    {
        throw new NotImplementedException();
    }
}