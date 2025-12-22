namespace Homework;

public class InMemoryToDoRepository : IToDoRepository
{
    private static List<ToDoItem> todoList = new();
    
    public async Task<IReadOnlyList<ToDoItem>> GetAllByUserId(Guid userId , CancellationToken ct)
    {
        return todoList.FindAll(item => item.User.UserId == userId);
    }

    public async Task<IReadOnlyList<ToDoItem>> GetActiveByUserId(Guid userId, CancellationToken ct)
    {
        return todoList.FindAll(item => item.User.UserId == userId && item.State == ToDoItem.ToDoItemState.Active);
    }

    public async Task<ToDoItem?> Get(Guid id, CancellationToken ct)
    {
        return todoList.Find(item => item.Id == id);
    }

    public async Task Add(ToDoItem item, CancellationToken ct)
    {
        todoList.Add(item);
    }

    public async Task Update(ToDoItem item, CancellationToken ct)
    {
        var index = todoList.IndexOf(item);
        if (index >= 0) todoList[index] = item;
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        todoList.RemoveAll(item => item.Id == id);
    }
    
    public async Task<bool> ExistsByName(Guid userId, string name, CancellationToken ct)
    {
        return todoList.Any(item => item.User.UserId == userId && item.Name == name);
    }
    
    public async Task<int> CountActive(Guid userId,  CancellationToken ct)
    {
        return GetActiveByUserId(userId, ct).Result.Count;
    }
    
    public async Task<int> CountAll(Guid userId, CancellationToken ct)
    {
        return GetAllByUserId(userId, ct).Result.Count;
    }

    public async Task<IReadOnlyList<ToDoItem>> Find(Guid userId, Func<ToDoItem, bool> predicate, CancellationToken ct)
    {
        return todoList.Where(predicate).ToList();
    }
}