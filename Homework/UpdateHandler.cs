using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

namespace Homework;

public class UpdateHandler : IUpdateHandler
{
    public void HandleUpdateAsync(ITelegramBotClient botClient, Update update)
    {
        // throw new NotImplementedException();
        
        switch (update)
        {
            case "/start":
                // botClient.SendMessage("111",  "");
                StartApp();
                break;
            case "/help":
                ShowHelp();
                break;
            case "/info":
                ShowInfo();
                break;
            case "/addtask":
                AddTask();
                break; 
            case "/completetask":
                CompleteTask();
                break;
            case "/showtasks":
                ShowTasks("Active");
                break;
            case "/showalltasks":
                ShowTasks("All");
                break;
        }
    }
}