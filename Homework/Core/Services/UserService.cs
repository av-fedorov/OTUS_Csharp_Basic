using Otus.ToDoList.ConsoleBot.Types;

namespace Homework;

public class UserService : IUserService
{   
    InMemoryUserRepository UserRepository = new();
    public async Task<ToDoUser> RegisterUser(long telegramUserId, string telegramUserName,  CancellationToken ct)
    {
        var User = new ToDoUser(telegramUserName, telegramUserId);
        await UserRepository.Add(User, ct);
        
        return User;
    }

    public async Task<ToDoUser>? GetUser(long telegramUserId,  CancellationToken ct)
    {
        return await UserRepository.GetUserByTelegramUserId(telegramUserId, ct);
    }
}