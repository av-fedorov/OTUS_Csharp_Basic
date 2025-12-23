namespace Homework;

public class TaskCountLimitException : Exception
{
    public TaskCountLimitException(int taskCountLimit)
    {
        Console.WriteLine($"Превышено максимальное количество задач равное {taskCountLimit}.");
    }
}