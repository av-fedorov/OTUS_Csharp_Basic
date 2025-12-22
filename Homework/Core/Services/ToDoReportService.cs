// namespace Homework;
//
// public class ToDoReportService : IToDoReportService
// {
//     ToDoService toDoService = new();
//     
//     public (int total, int completed, int active, DateTime generatedAt) GetUserStats(Guid userId)
//     {
//         var itemsList = toDoService.GetAllByUserId(userId, CancellationToken);
//             
//         return (
//             itemsList.Count(), 
//             itemsList.Count(item => item.State == ToDoItem.ToDoItemState.Completed),
//             itemsList.Count(item => item.State == ToDoItem.ToDoItemState.Active),
//             DateTime.Now);
//     }
// }