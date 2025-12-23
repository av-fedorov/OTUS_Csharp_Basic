namespace Homework;

public class ToDoService : IToDoService
{
    InMemoryToDoRepository toDoRepository = new();
    public int TaskCountLimit { get; set; }
    public int TaskLengthLimit { get; set; }
    
    public async Task<IReadOnlyList<ToDoItem>> GetAllByUserId(Guid userId, CancellationToken ct)
    {
        return await toDoRepository.GetAllByUserId(userId, ct);
    }

    public async Task<IReadOnlyList<ToDoItem>> GetActiveByUserId(Guid userId, CancellationToken ct)
    {
        return await toDoRepository.GetActiveByUserId(userId, ct);
    }

    public async Task<ToDoItem> Add(ToDoUser user, string name, CancellationToken ct)
    {
        if (await toDoRepository.CountAll(user.UserId, ct) >= TaskCountLimit)
            throw new ArgumentException($"Превышено максимальное количество задач ({TaskCountLimit} шт).");
        
        if (await toDoRepository.ExistsByName(user.UserId, name, ct))
            throw new ArgumentException($"Задача с указанным текстом ('{name}') уже существует в списке.");
        
        if (name.Length > TaskLengthLimit && TaskLengthLimit > 0) 
            throw new ArgumentException($"Длина задачи ({name.Length}) " +
                                        $"превышает максимально допустимое значение ({TaskLengthLimit}).");
        
        var newItem = new ToDoItem(user, name);
        await toDoRepository.Add(newItem, ct);

        return newItem;
    }

    public async Task MarkCompleted(Guid id, CancellationToken ct)
    {
        var item = toDoRepository.Get(id,  ct);
        
        if (item != null)
        {
            item.Result.State = ToDoItem.ToDoItemState.Completed;
            await toDoRepository.Update(item.Result, ct);
        }
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        await toDoRepository.Delete(id,  ct);
    }
    
    public async Task<IReadOnlyList<ToDoItem>> Find(ToDoUser user, string namePrefix, CancellationToken ct)
    {
        return await toDoRepository.Find(user.UserId, item => item.Name.StartsWith(namePrefix), ct);
    }
}