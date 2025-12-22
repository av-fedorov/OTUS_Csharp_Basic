namespace Homework;

public class ToDoReportService : IToDoReportService
{
    ToDoService toDoService = new();
    
    public async Task<(int total, int completed, int active, DateTime generatedAt)> GetUserStats(Guid userId, CancellationToken ct)
    {
        var itemsList = await toDoService.GetAllByUserId(userId, ct);
            
        return (
            itemsList.Count(), 
            itemsList.Count(item => item.State == ToDoItem.ToDoItemState.Completed),
            itemsList.Count(item => item.State == ToDoItem.ToDoItemState.Active),
            DateTime.Now);
    }
}