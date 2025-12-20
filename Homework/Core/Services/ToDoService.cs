namespace Homework;

public class ToDoService : IToDoService
{
    InMemoryToDoRepository ToDoRepository = new();
    public int TaskCountLimit { get; set; }
    public int TaskLengthLimit { get; set; }
    
    public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
    {
        return ToDoRepository.GetAllByUserId(userId);
    }

    public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
    {
        return ToDoRepository.GetActiveByUserId(userId);
    }

    public ToDoItem Add(ToDoUser user, string name)
    {
        if (ToDoRepository.CountAll(user.UserId) >= TaskCountLimit)
            throw new ArgumentException($"Превышено максимальное количество задач ({TaskCountLimit} шт).");
        
        if (ToDoRepository.ExistsByName(user.UserId, name))
            throw new ArgumentException($"Задача с указанным текстом ('{name}') уже существует в списке.");
        
        if (name.Length > TaskLengthLimit && TaskLengthLimit > 0) 
            throw new ArgumentException($"Длина задачи ({name.Length}) " +
                                        $"превышает максимально допустимое значение ({TaskLengthLimit}).");
        
        
        var newItem = new ToDoItem(user, name);
        ToDoRepository.Add(newItem);

        return newItem;
    }

    public void MarkCompleted(Guid id)
    {
        var item = ToDoRepository.Get(id);
        
        if (item != null)
        {
            item.State = ToDoItem.ToDoItemState.Completed;
            ToDoRepository.Update(item);
        }
    }

    public void Delete(Guid id)
    {
        ToDoRepository.Delete(id);
    }

    public IReadOnlyList<ToDoItem> Find(ToDoUser user, string namePrefix)
    {
        return ToDoRepository.Find(user.UserId, item => item.Name.StartsWith(namePrefix));
    }
}