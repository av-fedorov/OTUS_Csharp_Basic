namespace Homework;

public class ToDoReportService : IToDoReportService
{
    InMemoryToDoRepository ToDoRepository = new();
    
    public (int total, int completed, int active, DateTime generatedAt) GetUserStats(Guid userId)
    {
        return (
            ToDoRepository.GetAllByUserId(userId).Count(), 
            ToDoRepository.GetAllByUserId(userId).Count(item => item.State == ToDoItem.ToDoItemState.Completed),
            ToDoRepository.GetAllByUserId(userId).Count(item => item.State == ToDoItem.ToDoItemState.Active),
            DateTime.Now);
    }
}