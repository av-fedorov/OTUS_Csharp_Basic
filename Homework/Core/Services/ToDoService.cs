namespace Homework;

public class ToDoService : IToDoService
{
    InMemoryToDoRepository ToDoRepository = new();
    public int TaskCountLimit { get; set; }
    public int TaskLengthLimit { get; set; }
    
    public async Task<IReadOnlyList<ToDoItem>> GetAllByUserId(Guid userId, CancellationToken ct)
    {
        return await ToDoRepository.GetAllByUserId(userId, ct);
    }

    public async Task<IReadOnlyList<ToDoItem>> GetActiveByUserId(Guid userId, CancellationToken ct)
    {
        return await ToDoRepository.GetActiveByUserId(userId, ct);
    }

    public async Task<ToDoItem> Add(ToDoUser user, string name, CancellationToken ct)
    {
        if (await ToDoRepository.CountAll(user.UserId, ct) >= TaskCountLimit)
            throw new ArgumentException($"Превышено максимальное количество задач ({TaskCountLimit} шт).");
        
        if (ToDoRepository.ExistsByName(user.UserId, name, ct).Result)
            throw new ArgumentException($"Задача с указанным текстом ('{name}') уже существует в списке.");
        
        if (name.Length > TaskLengthLimit && TaskLengthLimit > 0) 
            throw new ArgumentException($"Длина задачи ({name.Length}) " +
                                        $"превышает максимально допустимое значение ({TaskLengthLimit}).");
        
        
        var newItem = new ToDoItem(user, name);
        await ToDoRepository.Add(newItem, ct);

        return newItem;
    }

    public async Task MarkCompleted(Guid id, CancellationToken ct)
    {
        var item = ToDoRepository.Get(id,  ct);
        
        if (item != null)
        {
            item.Result.State = ToDoItem.ToDoItemState.Completed;
            await ToDoRepository.Update(item.Result, ct);
        }
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        await ToDoRepository.Delete(id,  ct);
    }
    
    public async Task<IReadOnlyList<ToDoItem>> Find(ToDoUser user, string namePrefix, CancellationToken ct)
    {
        return await ToDoRepository.Find(user.UserId, item => item.Name.StartsWith(namePrefix), ct);
    }
}