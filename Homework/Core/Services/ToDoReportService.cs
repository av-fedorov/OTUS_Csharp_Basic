namespace Homework;

public class ToDoReportService : IToDoReportService
{
    ToDoService ToDoService = new ToDoService();
    
    public (int total, int completed, int active, DateTime generatedAt) GetUserStats(Guid userId)
    {
        var todoList = ToDoService.GetAllByUserId(userId);
            
        return (
            todoList.Count(), 
            todoList.Count(item => item.State == ToDoItem.ToDoItemState.Completed),
            todoList.Count(item => item.State == ToDoItem.ToDoItemState.Active),
            DateTime.Now);
    }
}