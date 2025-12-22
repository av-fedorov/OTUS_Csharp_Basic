using Otus.ToDoList.ConsoleBot.Types;
using Otus.ToDoList.ConsoleBot;


namespace Homework;

class Program
{
    static void Main()
    {
        try
        {
            using var cts = new CancellationTokenSource();
            var handler = new UpdateHandler();
            var botClient = new ConsoleBotClient();
            
            botClient.StartReceiving(handler, cts.Token);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Произошла непредвиденная ошибка: " +
                              $"{e.GetType()}: {e.Message} \n {e.StackTrace} \n {e.InnerException}");
        }
        
    }
}