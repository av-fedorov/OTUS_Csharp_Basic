namespace Homework;

public class ToDoService : IToDoService
{
    private static List<ToDoItem> todoList = new List<ToDoItem>();
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
        todoList.Add(new ToDoItem(user, name)); 
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