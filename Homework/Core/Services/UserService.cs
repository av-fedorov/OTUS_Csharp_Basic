using Otus.ToDoList.ConsoleBot.Types;

namespace Homework;

public class UserService : IUserService
{   
    InMemoryUserRepository UserRepository = new();
    public ToDoUser RegisterUser(long telegramUserId, string telegramUserName)
    {
        var User = new ToDoUser(telegramUserName, telegramUserId);
        UserRepository.Add(User);
        
        return User;
    }

    public ToDoUser? GetUser(long telegramUserId)
    {
        return UserRepository.GetUserByTelegramUserId(telegramUserId);
    }
}