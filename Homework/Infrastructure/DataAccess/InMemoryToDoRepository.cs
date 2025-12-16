namespace Homework;

public class InMemoryToDoRepository : IToDoRepository
{
    
    private readonly List<ToDoItem> todoList = [];
    // private static List<ToDoItem> todoList = new();
    
    public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
    {
        return todoList.FindAll(item => item.User.UserId == userId);
    }

    public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
    {
        return todoList.FindAll(item => item.User.UserId == userId && item.State == ToDoItem.ToDoItemState.Active);
    }

    public ToDoItem? Get(Guid id)
    {
        return todoList.Find(item => item.Id == id);
    }

    public void Add(ToDoItem item)
    {
        todoList.Add(item);
    }

    public void Update(ToDoItem item)
    {
        var index = todoList.IndexOf(item);
        
        if (index >= 0) todoList[index] = item;
    }

    public void Delete(Guid id)
    {
        todoList.RemoveAll(item => item.Id == id);
    }
    
    public bool ExistsByName(Guid userId, string name)
    {
        return todoList.Any(item => item.User.UserId == userId && item.Name == name);
    }
    
    public int CountActive(Guid userId)
    {
        return GetActiveByUserId(userId).Count;
    }
    
    public int CountAll(Guid userId)
    {
        return GetAllByUserId(userId).Count;
    }

    public IReadOnlyList<ToDoItem> Find(Guid userId, Func<ToDoItem, bool> predicate)
    {
        return todoList.Where(predicate).ToList();
    }
}