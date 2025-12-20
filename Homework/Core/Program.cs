using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;


namespace Homework;

class Program
{
    // Домашнее задание №16: Интерфейсы
    static void Main()
    {
        try
        {
            var handler = new UpdateHandler();
            var botClient = new ConsoleBotClient();
        
            botClient.StartReceiving(handler);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Произошла непредвиденная ошибка: " +
                              $"{e.GetType()}: {e.Message} \n {e.StackTrace} \n {e.InnerException}");
        }
        
    }
}