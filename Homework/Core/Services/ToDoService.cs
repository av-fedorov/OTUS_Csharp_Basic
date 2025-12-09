namespace Homework;

public class ToDoService : IToDoService
{
    private static List<ToDoItem> todoList = new();
    public int TaskCountLimit { get; set; }
    public int TaskLengthLimit { get; set; }
    
    public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
    {
        return todoList.FindAll(item => item.User.UserId == userId);
    }

    public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
    {
        return todoList.FindAll(item => item.User.UserId == userId && item.State == ToDoItem.ToDoItemState.Active);
    }

    public ToDoItem Add(ToDoUser user, string name)
    {
        if (todoList.Count >= TaskCountLimit)
            throw new ArgumentException($"Превышено максимальное количество задач ({TaskCountLimit} шт).");
        if (name.Length > TaskLengthLimit && TaskLengthLimit > 0) 
            throw new ArgumentException($"Длина задачи ({name.Length}) " +
                                        $"превышает максимально допустимое значение ({TaskLengthLimit}).");
        if (todoList.Any(item => item.Name == name))
            throw new ArgumentException($"Задача с указанным текстом ('{name}') уже существует в списке.");
        
        var newItem = new ToDoItem(user, name);
        todoList.Add(newItem);

        return newItem;
    }

    public void MarkCompleted(Guid id)
    {
        todoList.Find(item => item.Id == id).State = ToDoItem.ToDoItemState.Completed;
        todoList.Find(item => item.Id == id).StateChangedAt = DateTime.Now;
    }

    public void Delete(Guid id)
    {
        todoList.Remove(todoList.Find(item => item.Id == id));
    }
}