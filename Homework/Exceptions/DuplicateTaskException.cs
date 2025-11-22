namespace Homework;

public class DuplicateTaskException : Exception
{
    public DuplicateTaskException(string task)
    {
        Console.WriteLine($"Задача '{task}' уже существует.");
    }
}