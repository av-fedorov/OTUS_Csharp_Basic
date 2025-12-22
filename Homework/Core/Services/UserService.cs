using Otus.ToDoList.ConsoleBot.Types;

namespace Homework;

public class UserService : IUserService
{   
    InMemoryUserRepository userRepository = new();
    public async Task<ToDoUser> RegisterUser(long telegramUserId, string telegramUserName,  CancellationToken ct)
    {
        var user = new ToDoUser(telegramUserName, telegramUserId);
        await userRepository.Add(user, ct);
        
        return user;
    }

    public async Task<ToDoUser>? GetUser(long telegramUserId,  CancellationToken ct)
    {
        return await userRepository.GetUserByTelegramUserId(telegramUserId, ct);
    }
}