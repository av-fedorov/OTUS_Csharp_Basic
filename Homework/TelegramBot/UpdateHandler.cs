using System.Globalization;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

namespace Homework;

public class UpdateHandler : IUpdateHandler
{
    
    // private delegate void MessageEventHandler(string message);
    // private event MessageEventHandler OnHandleUpdateStarted;
    // private event MessageEventHandler OnHandleUpdateCompleted;
    
    private readonly string command;
    private static ToDoUser CurrentUser;
    private static IReadOnlyList<ToDoItem> todoList = new List<ToDoItem>();
    private readonly ToDoService TaskService = new();
    private readonly UserService UserService = new();
    // private readonly ToDoReportService ReportService = new();
    private readonly string versionText = "Версия программы:    03.19.02 " +
                                        "\nДата создания:       10.12.2025 " +
                                        "\nДата обновления:     16.12.2025";
    
    
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        // OnHandleUpdateStarted += DisplayMessage;
        // OnHandleUpdateCompleted += DisplayMessage;
        
        try
        {
            // OnHandleUpdateStarted?.Invoke($"Началась обработка сообщения '{update.Message.Text}'.");
            RunApp();
            // OnHandleUpdateCompleted?.Invoke($"Закончилась обработка сообщения '{update.Message.Text}'.");
        }
        catch (ArgumentException e)
        {
            botClient.SendMessage(update.Message.Chat, e.Message, ct);
        }
        catch (Exception e)
        {
            botClient.SendMessage(update.Message.Chat, e.Message, ct);
        }
        finally
        {
            // OnHandleUpdateStarted -= DisplayMessage;
            // OnHandleUpdateCompleted -= DisplayMessage;
        }

        void RunApp()
        {
             botClient.SendMessage(update.Message.Chat, "Список доступных команд:", ct);
             botClient.SendMessage(update.Message.Chat, " /start \n /help \n /info \n " +
                                                       "/addtask \n /showtasks \n /showalltasks /find \n " +
                                                       "/completetask /removetask /report \n" +
                                                       "/exit\n", ct);

            botClient.SendMessage(update.Message.Chat, $"Вы ввели: '{update.Message.Text}'\n", ct);
            // await botClient.SendMessage(update.Message.Chat, $"Получил '{update.Message.Text}'", ct);
            
            switch (update.Message.Text)
            {
                case "/start":
                    StartApp();
                    break;
                case "/help":
                    ShowHelp();
                    break;
                case "/info":
                    ShowInfo();
                    break;
                case string cmd when cmd.StartsWith("/addtask") && CanRunCmd():
                    AddTask();
                    break;
                case string cmd when cmd.StartsWith("/completetask") && CanRunCmd():
                    CompleteTask();
                    break;
                case string cmd when cmd.StartsWith("/removetask") && CanRunCmd():
                    RemoveTask();
                    break;
                case string cmd when cmd == "/showtasks" && CanRunCmd():
                    ShowTasks("active");
                    break;
                case string cmd when cmd == "/showalltasks" && CanRunCmd():
                    ShowTasks();
                    break;
                // case string cmd when cmd == "/report" && CanRunCmd():
                //     // ShowReport();
                //     break;
                case string cmd when cmd.StartsWith("/find") && CanRunCmd():
                    FindTask();
                    break;
            }
        }

        ///////////////////////
        // Команда запуска/инициализации приложения
        void StartApp()
        {
            CurrentUser = UserService.GetUser(update.Message.From.Id, ct).Result;
            
            if (CurrentUser is null)
                CurrentUser = UserService.RegisterUser(update.Message.From.Id, update.Message.From.Username, ct).Result;
            
            botClient.SendMessage(update.Message.Chat, $"Здравствуйте {CurrentUser.TelegramUserName}.", ct);

            botClient.SendMessage(update.Message.Chat,
                "Введите максимально допустимое количество задач (от 1 до 100): ", ct);
            var taskCountLimit = Console.ReadLine();

            Console.WriteLine($"$taskCountLimit = {taskCountLimit}");
            
            TaskService.TaskCountLimit = ParseAndValidateInt(taskCountLimit, 1, 100);
            Console.WriteLine($"ParseAndValidateInt: ${ParseAndValidateInt(taskCountLimit, 1, 100)}" );
            
            botClient.SendMessage(update.Message.Chat,
                "Введите максимально допустимую длину задачи: ", ct);
            TaskService.TaskLengthLimit = ParseAndValidateInt(Console.ReadLine(), 1, 255);
            
            botClient.SendMessage(update.Message.Chat,
                "Параметры работы приложения заданы. Введите команду для продолжения работы.", ct);
        }

        ///////////////////////
        // Отображение версии/информации о приложении
        void ShowInfo()
        {
            botClient.SendMessage(update.Message.Chat, versionText, ct);
        }

        ///////////////////////
        // Отображение доступных команд 
        void ShowHelp()
        {
            botClient.SendMessage(update.Message.Chat, "Список доступных команд:", ct);
            botClient.SendMessage(update.Message.Chat, " /start          - начало работы с программой.", ct);
            botClient.SendMessage(update.Message.Chat, " /help           - вывод описания по доступным командам.", ct);
            botClient.SendMessage(update.Message.Chat, " /info           - техническая информация о программе.", ct);
            botClient.SendMessage(update.Message.Chat, " /showtasks      - отобразить все задачи из списка дел.", ct);
            botClient.SendMessage(update.Message.Chat, " /showalltasks   - отобразить задачи только в статусе 'Active'.", ct);
            botClient.SendMessage(update.Message.Chat, " /addtask текст  - добавить задачу в список дел.", ct);
            botClient.SendMessage(update.Message.Chat, " /completetask № - завершить выбранную задачу.", ct);
            botClient.SendMessage(update.Message.Chat, " /removetask №   - удалить выбранную задачу.", ct);
            botClient.SendMessage(update.Message.Chat, " /find текст     - найти задачи, текст которых начинается с указанного выражения.", ct);
            botClient.SendMessage(update.Message.Chat, " /report         - отобразить сводный отчет по задачам.", ct);
            botClient.SendMessage(update.Message.Chat, " /exit           - выход из программы.", ct);
        }

        ///////////////////////
        // Отображение списка задач. Параметр mode определяет условия фильтрации задач в списке
        // 'all' = все задачи, 'active' = только активные. 
        bool ShowTasks(string mode = "all")
        {
            if (mode == "active")
                todoList = TaskService.GetActiveByUserId(CurrentUser.UserId, ct).Result;
            else
                todoList = TaskService.GetAllByUserId(CurrentUser.UserId, ct).Result;

            if (todoList.Count == 0)
                botClient.SendMessage(update.Message.Chat, "Список задач пуст.", ct);
            else
            {
                for (int i = 0; i < todoList.Count; i++)
                {
                    botClient.SendMessage(update.Message.Chat,
                        $"#{i + 1}. {todoList[i].CreatedAt} [{todoList[i].Id}] " +
                        $"\nCтатус: [{todoList[i].State}], изменен: {todoList[i].StateChangedAt}" +
                        $"\nТекст: {todoList[i].Name}\n", ct);
                }
            }

            return todoList.Count > 0;
        }

        ///////////////////////
        // Добавление задач в список. Текст задачи указывается после команды.
        void AddTask()
        {
            var taskText = update.Message.Text.Substring("/addtask".Length).Trim();
            ValidateString(taskText);
            
            if (!string.IsNullOrWhiteSpace(taskText))
            {
                TaskService.Add(CurrentUser, taskText, ct);
                botClient.SendMessage(update.Message.Chat, "Задача добавлена.", ct);
            }
            else
                botClient.SendMessage(update.Message.Chat, "Текст задачи не задан.", ct);
        }

        ///////////////////////
        // Завершение задачи. Номер завершаемой задачи указывается после команды.
        void CompleteTask()
        {
            if (ShowTasks("active"))
            {
                todoList = TaskService.GetActiveByUserId(CurrentUser.UserId, ct).Result;
                int.TryParse(update.Message.Text.Substring("/completetask".Length).Trim(), out var taskNumber);

                if (taskNumber > 0 && taskNumber <= todoList.Count)
                {
                    TaskService.MarkCompleted(todoList[taskNumber - 1].Id, ct);
                    botClient.SendMessage(update.Message.Chat, $"Задача №{taskNumber} завершена.", ct);
                }
                else
                    botClient.SendMessage(update.Message.Chat, $"Задача №{taskNumber} отсутствует в списке.", ct);
            }
        }

        ///////////////////////
        // Удаление задачи. Номер удаляемой задачи указывается после команды.
        void RemoveTask()
        {
            if (ShowTasks("active"))
            {
                todoList = TaskService.GetActiveByUserId(CurrentUser.UserId, ct).Result;
                int.TryParse(update.Message.Text.Substring("/removetask".Length).Trim(), out var taskNumber);

                if (taskNumber > 0 && taskNumber <= todoList.Count)
                {
                    TaskService.Delete(todoList[taskNumber - 1].Id, ct);
                    botClient.SendMessage(update.Message.Chat, $"Задача №{taskNumber} удалена.", ct);
                }
                else
                    botClient.SendMessage(update.Message.Chat, $"Задача №{taskNumber} отсутствует в списке.", ct);
            }
        }

        ///////////////////////
        // Поиск задачи по предикату. Предикат указывается после команды.
        void FindTask()
        {
            var namePrefix = update.Message.Text.Substring("/find".Length).Trim();
            ValidateString(namePrefix);
            
            todoList = TaskService.Find(CurrentUser, namePrefix, ct).Result;
            
            // TODO: объединить вывод задач из этого метода и ShowTasks() в отдельный общий метод.
            if (todoList.Count == 0)
                botClient.SendMessage(update.Message.Chat, 
                    "Отсутствуют задачи соответствующие критериям поиска", ct);
            else
            {
                for (int i = 0; i < todoList.Count; i++)
                {
                    botClient.SendMessage(update.Message.Chat,
                        $"#{i + 1}. {todoList[i].CreatedAt} [{todoList[i].Id}] " +
                        $"\nCтатус: [{todoList[i].State}], изменен: {todoList[i].StateChangedAt}" +
                        $"\nТекст: {todoList[i].Name}\n", ct);
                }
            }
        }
        
        ///////////////////////
        // Отображение сводного отчета о задачах.
        // void ShowReport()
        // {
        //     var report = ReportService.GetUserStats(CurrentUser.UserId);
        //     botClient.SendMessage(update.Message.Chat,
        //         $"Статистика по задачам на {report.generatedAt}. " +
        //         $"Всего: {report.total}; Завершенных: {report.completed}; Активных: {report.active}.", ct);
        // }

        ///////////////////////
        // Проверка возможности запуска необходимой команды.
        bool CanRunCmd()
        {
            if (CurrentUser != null)
                return true;
            else
            {
                botClient.SendMessage(update.Message.Chat,
                    $"Использование команды недоступно, т.к. пользователь не зарегистрирован." +
                    $"\n Необходимо выполнить команду '/start'.\n", ct);

                return false;
            }
        }
        
        int ParseAndValidateInt(string? str, int min, int max)
        {
            int number;
            int.TryParse(str, out number);
            
            if (number < min || number > max)
                throw new ArgumentException($"Строка не является числом, либо выходит за пределы указанного диапазона от {min} до {max}.");
        
            return number;
        }
    
        void ValidateString(string? str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)) 
                throw new ArgumentException("Введенное значение не должно быть равно null, пустой строке или пробелу.");
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
    {
        Console.WriteLine($"HandleError: {exception})");
        return Task.CompletedTask;
    }
    
    void DisplayMessage(string message) => Console.WriteLine(message);
}