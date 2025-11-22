using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;


namespace Homework;

class Program
{
    // Домашнее задание №16: Интерфейсы
    static void Main()
    {
        Console.WriteLine("Добро пожаловать в консольного бота :) \n");
        Console.WriteLine("Список доступных команд:");
        Console.WriteLine(" /start \n /help \n /info \n " +
                          "/addtask \n /showtasks \n /showalltasks \n /completetask \n " +
                          "/exit");
        
        var handler = new UpdateHandler();
        var botClient = new ConsoleBotClient();
        
        botClient.StartReceiving(handler);
    }
}