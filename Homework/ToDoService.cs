namespace Homework;

public class ToDoService : IToDoService
{
    public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }

    public ToDoItem Add(ToDoUser user, string name)
    {
        string taskText = "";

        do
        {
            botClient.SendMessage(update.Message.Chat, "Введите описание задачи: ");
            taskText = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(taskText));

        todoList.Add(new ToDoItem(User, taskText));
        botClient.SendMessage(update.Message.Chat, "\nЗадача добавлена.");
        
        // throw new NotImplementedException();
    }

    public void MarkCompleted(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}